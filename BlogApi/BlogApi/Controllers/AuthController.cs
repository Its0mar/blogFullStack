using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IFileService _fileService;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IFileService fileService, IJwtService jwtService )

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fileService = fileService;
            _jwtService = jwtService;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO dto, [FromQuery]string? returnUrl = null)
        {            
            var user = dto.ToApplicationUser();
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return Problem(statusCode: 400, title: "Failed to register", detail:string.Join(" ", result.Errors.Select(e => e.Description)));
            }
            if (dto.ProfilePic != null)
            {
                try
                {
                    string[] exts = { ".png", ".jpeg", ".jpg" };
                    var filePath = await _fileService.UploadFileAsync(dto.ProfilePic, exts, 3, "ProfilePic");
                    user = await _userManager.FindByNameAsync(dto.UserName);
                    if (user == null)
                    {
                        return Problem(statusCode: 500, title: "Error Occured", detail: "Error while getting user");
                    }
                    user.ProfilePicPath = filePath;
                    await _userManager.UpdateAsync(user);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            if (await _roleManager.FindByNameAsync("User") is null)
            {
                IdentityRole<Guid> applicationRole = new IdentityRole<Guid>() { Name = "User" };
                await _roleManager.CreateAsync(applicationRole);
            }
            await _userManager.AddToRoleAsync(user, "User");
            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home", Id = getCurrentUserId() });

        }

        [HttpPost]
        //[AllowAnonymous]
        [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] string? returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user == null)
                return Unauthorized("Invalid credentials");
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (result == false)
                return Unauthorized("Invalid credentials");

            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home" });
        }

        #region UtilityMethod
        private Guid getCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return (id == null ? Guid.Empty : Guid.Parse(id));
        }
        #endregion

    }
}
