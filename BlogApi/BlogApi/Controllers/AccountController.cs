using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(UserManager<ApplicationUser> userManager, IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
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
    }
}
