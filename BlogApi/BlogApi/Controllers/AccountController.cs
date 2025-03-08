using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
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

        public AccountController(UserManager<ApplicationUser> userManager, IAccountService accountService, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // update acccount info

        [HttpPost]
        public async Task<IActionResult> UpdateAccountInfo(UpdateAccInfoDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId ?? Guid.Empty.ToString());
            if (user == null)
            {
                return NotFound("user is not found");
            }
            try
            {
                var result = await _accountService.UpdateAccountInfoAsync(user, dto);
                if (!result)
                {
                    return BadRequest("update info failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return NotFound("user with this email or username is not found");

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
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully.");
        }

        // delete account
        [HttpDelete] 
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.FindByNameAsync(User?.Identity?.Name ?? "");
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("the result is not succeeded");
            }
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult Meow()
        {
            return Ok("hello heloo");
        }
    }
}
