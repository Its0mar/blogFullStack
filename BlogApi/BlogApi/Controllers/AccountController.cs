using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.DTO.PostDTOS;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        // to delete later
        private readonly IPostService _postService;

        public AccountController(UserManager<ApplicationUser> userManager, IAccountService accountService, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, RoleManager<IdentityRole<Guid>> roleManager, IPostService postService)
        {
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _postService = postService;
        }

        // update acccount info

        [HttpPost]
        public async Task<IActionResult> UpdateAccountInfo(UpdateAccInfoDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId ?? Guid.Empty.ToString());
            if (user == null)
            {
                return Problem(statusCode:404, title:"User not found");
            }
            var checkPassowrd = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            if (checkPassowrd == false)
                return Problem( title: "Forbidden" , detail: "password is not correct", statusCode: StatusCodes.Status403Forbidden);

            try
            {
                var result = await _accountService.UpdateAccountInfoAsync(user, dto);
                if (!result)
                {
                    return Problem(title:"Failed", detail:"Failed to update password", statusCode:StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return Problem(title:"Failed", detail: ex.Message, statusCode:StatusCodes.Status500InternalServerError);
            }
            
            return Ok();

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);
            if (user == null)
                await _userManager.FindByNameAsync(dto.EmailOrUserName);

            if (user == null || user.Email == null)
                return Problem(title: "Account not found", detail: "email or password is incorrect", statusCode: StatusCodes.Status404NotFound);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callBackUrl = Url.Action("ResetPassword", "Account", new { token, email = dto.EmailOrUserName }, Request.Scheme);
            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callBackUrl}'>link</a>");
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok("Reset password link has been sent to your email.");
            
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassDTO dto)
        {
            var result = await _accountService.UpdatePasswordAsync(dto,getCurrentUserId(), dto.NewPassword);
            if (!result.Succeeded)
                return Problem(title: "Failed", detail:String.Join(", ",result.Errors.Select(e => e.Description).ToList()));

            return Ok("Password updated successfully");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Problem(title: "Account not found", detail: "email is incorrect", statusCode: StatusCodes.Status404NotFound);

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);

            if (!result.Succeeded)
                return Problem(title: "Reset failed", detail: string.Join(",", result.Errors.Select(e => e.Description)), statusCode: StatusCodes.Status500InternalServerError);

            return Ok("Password has been reset successfully.");
        }

        // delete account
        [HttpDelete] 
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.FindByNameAsync(User?.Identity?.Name ?? "");
            if (user == null)
            {
                return Problem(title: "Account not found", detail:"Account not found", statusCode: StatusCodes.Status404NotFound);

            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Problem(title:"Error", detail:"An error occured", statusCode:StatusCodes.Status500InternalServerError);
            }
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet("/")]
        [AllowAnonymous]
        public async Task<IActionResult> Meow()
        {
            var normalUser = new ApplicationUser
            {
                UserName = "user",
                PersonName = "User",
                NormalizedUserName = "USER@EXAMPLE.COM",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true
            };
            if (!await _roleManager.RoleExistsAsync("User")) {
                IdentityRole<Guid> role = new IdentityRole<Guid>() { Name = "User" };
                await _roleManager.CreateAsync(role);

            }
            await _userManager.CreateAsync(normalUser, "user34");
            await _userManager.AddToRoleAsync(normalUser, "User");
           // await _signInManager.SignInAsync(normalUser, true);

            AddPostDTO post = new AddPostDTO
            {
                AuthorId = getCurrentUserId(),
                Body = "bodyyy",
                Title = "titelee",
                IsPublic = true,
            };
            await _postService.AddPostAsync(post);
            return Ok("hello heloo");
        }

        private Guid getCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return (id == null ? Guid.Empty : Guid.Parse(id));
        }
    }
}
