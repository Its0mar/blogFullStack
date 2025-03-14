
using System.ComponentModel.DataAnnotations;
using ZeroBlog.Core.Domain.Entities;

namespace ZeroBlog.Core.DTO.PostDTOS
{
    public class AddPostDTO
    {
        [Required(ErrorMessage = "Title can`t be empty")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Body can`t be empty")]
        public string Body { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsPublic { get; set; } = true;


        public Post ToPost()
        {
            return new Post { Title = this.Title, Body = this.Body, AuthorId = this.AuthorId, CreatedDate = this.CreatedDate, IsPublic = this.IsPublic };
        }
    }
}
