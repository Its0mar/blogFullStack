

using System.ComponentModel.DataAnnotations;

namespace ZeroBlog.Core.DTO
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "This Field Is Required")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; } = string.Empty;
        public bool IsPersistent { get; set; } = true;

    }
}
