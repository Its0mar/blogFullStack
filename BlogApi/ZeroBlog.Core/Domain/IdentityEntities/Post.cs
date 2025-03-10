using System.ComponentModel.DataAnnotations;

namespace ZeroBlog.Core.Domain.IdentityEntities
{
    public class Post : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set; }

        public ApplicationUser Author { get; set; } = new();
    }
}
