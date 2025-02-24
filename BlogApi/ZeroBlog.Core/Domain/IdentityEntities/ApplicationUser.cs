using Microsoft.AspNetCore.Identity;


namespace ZeroBlog.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PersonName { get; set; } = string.Empty;
        public string? ProfilePicPath { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
