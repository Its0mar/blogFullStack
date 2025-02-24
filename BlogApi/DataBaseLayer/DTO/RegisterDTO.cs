
using System.ComponentModel.DataAnnotations;

namespace DataBaseLayer.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person Name Is Required")]
        public string PersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } 
        public string? ProfilePicPath { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Paasowrd And Confrim Pasword Does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
