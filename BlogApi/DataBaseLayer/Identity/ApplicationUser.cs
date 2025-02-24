using Microsoft.AspNetCore.Identity;

namespace DataBaseLayer.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PersonName { get; set; } = string.Empty;
        public string ProfilePicPath { get; set; } = string.Empty;
    }
}
