
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.DTO
{
    public class UpdateAccInfoDTO
    {

        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get; set; }
        public string? ProfilePicPath { get; set; }
        public IFormFile? ProfilePic { get; set; }


        public ApplicationUser ToApplicationUser(ApplicationUser user)
        {
            user.PersonName = this.PersonName;
            user.UserName = this.UserName;
            user.Email = this.Email;
            user.PhoneNumber = this.PhoneNumber;
            user.ProfilePicPath = this.ProfilePicPath;

            return user;
        }
    }
}
