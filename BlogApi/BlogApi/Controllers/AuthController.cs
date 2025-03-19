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
        [AllowAnonymous]
      //  [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO dto, [FromQuery]string? returnUrl = null)
        {
            
            var user = dto.ToApplicationUser();
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (dto.ProfilePic != null)
            {
                try
                {
                    string[] exts = { ".png", ".jpeg", ".jpg" };
                    var filePath = await _fileService.UploadFileAsync(dto.ProfilePic, exts, 3, "ProfilePic");
                    //dto.ProfilePicPath = filePath;
                    user = await _userManager.FindByNameAsync(dto.UserName);
                    if (user == null)
                    {
                        return BadRequest("Error Occured");
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
            await _signInManager.SignInAsync(user, isPersistent: dto.IsPersistent);
            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home" });

        }

        [HttpPost]
        [AllowAnonymous]
       // [Authorize(policy: "NotAuthenticatedPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] string? returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user == null)
                return Unauthorized("Invalid credentials");
           // var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: dto.IsPersistent, true);
           // if (!result.Succeeded) return Unauthorized("Invalid Email-User Name or password");

            var token = _jwtService.CreateJwtToken(user);
            return Ok(new { Token = token, ReturnUrl = returnUrl ?? "/home" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }


        #region UtilityMethod
        private string GenerateJWT(ApplicationUser user)
        {
            Env.Load();
            string secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "secret_key_key_secret";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken("https://localhost:7210/",
                "https://localhost:7210/",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        #endregion

    }
}
