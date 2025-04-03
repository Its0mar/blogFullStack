
using Microsoft.AspNetCore.Identity;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        public AccountService(UserManager<ApplicationUser> userManager, IFileService fileService)
        {
            _userManager = userManager;
            _fileService = fileService;
        }

        public async Task<bool> UpdateAccountInfoAsync(ApplicationUser user, UpdateAccInfoDTO dto)
        {
            var pathToDelete = user.ProfilePicPath;
            user = dto.ToApplicationUser(user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return false;
            }
            string[] exts = { ".png", ".jpeg", ".jpg" };
            if (dto.ProfilePic != null)
            {
                if (pathToDelete != null)
                {
                    try
                    {
                        _fileService.DeleteFile(pathToDelete);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                try
                {
                    var path = await _fileService.UploadFileAsync(dto.ProfilePic, exts, 3, "ProfilePic");
                    user.ProfilePicPath = path;
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }

            else if (dto.ProfilePic == null)
            {
                if (pathToDelete != null)
                {
                    try
                    {
                        _fileService.DeleteFile(pathToDelete);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return true;

        }

        public async Task<IdentityResult> UpdatePasswordAsync(UpdatePassDTO dto, Guid Id,string newPassword)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User is not found" });
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            if (!checkPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Old Password is incorrect" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }
    }
}
