using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.Domain.Entities
{
    public class UserSavedPost : BaseEntity
    {
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; }
        public Guid PostID { get; set; }
        public Post Post { get; set; }
        public DateTime SavedAt { get; set; } 
    }
}
