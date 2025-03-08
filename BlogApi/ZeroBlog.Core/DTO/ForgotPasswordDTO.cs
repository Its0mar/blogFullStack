

using System.ComponentModel.DataAnnotations;

namespace ZeroBlog.Core.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}
