
using System.ComponentModel.DataAnnotations;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.DTO
{
    public class RegisterDTO
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
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Cofirm password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsPersistent { get; set; } = true;


        public ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid(),
                PersonName = this.PersonName,
                UserName = this.UserName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                ProfilePicPath = this.ProfilePicPath,
                CreationDate = DateTime.UtcNow
            };
        }

    }
}
