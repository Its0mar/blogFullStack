
using Microsoft.AspNetCore.Identity;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Core.Services
{
    public class AccountService :IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        public AccountService(UserManager<ApplicationUser> userManager, IFileService fileService)
        { 
            _userManager = userManager;
            _fileService = fileService;
        }

        public async Task<bool> UpdateAccountInfoAsync(ApplicationUser user,UpdateAccInfoDTO dto)
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
    }
}
