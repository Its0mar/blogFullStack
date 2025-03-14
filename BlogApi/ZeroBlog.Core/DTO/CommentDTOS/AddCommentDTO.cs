
using System.ComponentModel.DataAnnotations;

namespace ZeroBlog.Core.DTO.CommentDTOS
{
    public class AddCommentDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Comment length range is [5-100]")]
        public string Content { get; set; } = "";
        public Guid? ParentCommentId {  get; set; }
        public Guid PostId {  get; set; }
    }
}
