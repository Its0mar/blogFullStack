
using System.ComponentModel.DataAnnotations;

namespace ZeroBlog.Core.DTO
{
    public class UpdatePassDTO
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm New Password is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
