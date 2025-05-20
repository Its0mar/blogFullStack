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
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, IAccountService accountService, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, RoleManager<IdentityRole<Guid>> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAccountInfo(UpdateAccInfoDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return Problem(title: "Unauthorized", detail: "User not authenticated", statusCode:StatusCodes.Status401Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(GetCurrentUserId().ToString());
            if (user is null)
            {
                return Problem(title: "User not found", detail: "The requested user account was not found", statusCode: StatusCodes.Status404NotFound);
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            if (!checkPassword)
                return Problem( title: "Invalid Password", detail: "Current password is not correct", statusCode: StatusCodes.Status400BadRequest);

            try
            {
                var result = await _accountService.UpdateAccountInfoAsync(user, dto);
                if (!result)
                {
                    return Problem( title: "Update Failed", detail: "Failed to update account information", statusCode: StatusCodes.Status500InternalServerError);
                }
                return Ok("Account information updated successfully");
            }
            catch (Exception)
            {
                return Problem( title: "updating the picture failed", detail: "An error occurred while updating the profile picture", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize("NotAuthenticatedPolicy")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);
            if (user is null)
                user = await _userManager.FindByNameAsync(dto.EmailOrUserName);

            if (user is null || string.IsNullOrEmpty(user.Email))
                return Problem(title: "Account not found", detail: "email or password is incorrect", statusCode: StatusCodes.Status404NotFound);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(user.Email);
            var callBackUrl = $"{Request.Scheme}://{Request.Host}/ResetPassword?token={encodedToken}&email={encodedEmail}";

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callBackUrl}'>link</a>");
                return Ok("Reset password link has been sent to your email.");
            }
            catch(Exception ex)
            {
                // TODO : log the exception
                return Problem( title: "Email Sending Failed", detail: "Failed to send password reset email", statusCode: StatusCodes.Status500InternalServerError);
            }
            
            
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassDTO dto)
        {
            var result = await _accountService.UpdatePasswordAsync(dto,GetCurrentUserId(), dto.NewPassword);
            if (!result.Succeeded)
                return Problem(title: "Failed", detail:String.Join(", ",result.Errors.Select(e => e.Description).ToList()));

            return Ok("Password updated successfully");
        }

        [HttpPatch]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {

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
            var user = await _userManager.FindByNameAsync(User.Identity?.Name ?? "");
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

            return Ok(new { Message = "Account deleted successfully" });
        }
        private Guid GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }
}
