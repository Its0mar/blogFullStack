using Microsoft.AspNetCore.Identity;
using ZeroBlog.Core.Domain.Entities;


namespace ZeroBlog.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PersonName { get; set; } = string.Empty;
        public string? ProfilePicPath { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Post> UserPosts { get; set; } = new();
        public List<UserSavedPost> SavedPosts { get; set; } = new();
    }
}
