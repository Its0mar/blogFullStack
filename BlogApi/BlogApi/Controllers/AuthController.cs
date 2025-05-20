using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
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
        [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO dto, [FromQuery]string? returnUrl = null)
        {            
            var user = dto.ToApplicationUser();
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return Problem(statusCode: 400, title: "Failed to register", detail:string.Join(" ", result.Errors.Select(e => e.Description)));
           
            try
            {
                await ProcessProfilePictureAsync(dto, user);
            }
            catch (Exception ex)
            {
                //return Problem(title: "Failed to upload profile picture", detail: ex.Message, statusCode: 500);
                // TODO: Log
            }

            
            await AssignUserRoleAsync(user);
            
            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home", Id = GetCurrentUserId() });

        }

        [HttpPost]
        [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] string? returnUrl = null)
        {
            var user = await FindUserByEmailOrUsernameAsync(dto.UserNameOrEmail);

            if (user == null || await _userManager.CheckPasswordAsync(user, dto.Password))
                return Problem(statusCode: 401, title: "Invalid credentials", detail: "Invalid credentials");

            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home" });
        }

        #region UtilityMethod

        private async Task<ApplicationUser?> FindUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            var user = await _userManager.FindByEmailAsync(emailOrUsername) ?? await _userManager.FindByNameAsync(emailOrUsername);
            return user;
        }

        private async Task ProcessProfilePictureAsync(RegisterDTO dto, ApplicationUser user)
        {
            if (dto.ProfilePic is null) return;

            string[] allowedExtensions = { ".png", ".jpeg", ".jpg" };
            var filePath = await _fileService.UploadFileAsync(dto.ProfilePic, allowedExtensions, 3, "ProfilePic");
            var refreshedUser = await _userManager.FindByNameAsync(dto.UserName);
            
            if (refreshedUser is null) 
                throw new InvalidOperationException("User not found after creation");
            
            refreshedUser.ProfilePicPath = filePath;
            var updateResult = await _userManager.UpdateAsync(refreshedUser);
            
            if (!updateResult.Succeeded)
                throw new Exception($"Failed to update user profile picture: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
        }
        
        private async Task AssignUserRoleAsync(ApplicationUser user)
        {
            const string userRoleName = "User";
            
            if (await _roleManager.FindByNameAsync(userRoleName) is null)
            {
                var applicationRole = new IdentityRole<Guid>(userRoleName);
                await _roleManager.CreateAsync(applicationRole);
            }

            await _userManager.AddToRoleAsync(user, userRoleName);
        }
        private Guid GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var result) ? result : Guid.Empty;
        }
        #endregion

    }
}
