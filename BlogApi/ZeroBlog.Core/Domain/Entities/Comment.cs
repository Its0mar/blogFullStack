
using System.ComponentModel.DataAnnotations;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.Domain.Entities
{
    public class Comment : BaseEntity
    {
        [Key]
        public Guid Id {  get; set; }
        public String Content { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set;}
        public Guid AuthorId {  get; set; }
        public Guid PostID {  get; set; }
        public Guid? ParentCommentId {  get; set; }
        public int NumberOfLikes { get; set; } = 0;
        public int NumberOfDisLikes { get; set; } = 0;

        public ApplicationUser Author { get; set; }
        public Comment? ParentComment { get; set; }
        public Post Post { get; set; } = new();
        public List<Comment> Replies { get; set; }

        //public Comment()
        //{
        //    Author = new();
        //    Replies = new();
        //}


    }
}
