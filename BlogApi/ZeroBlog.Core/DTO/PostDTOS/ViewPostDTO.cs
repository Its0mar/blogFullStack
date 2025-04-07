
using Microsoft.AspNetCore.Http;
using ZeroBlog.Core.Domain.Entities;

namespace ZeroBlog.Core.DTO.PostDTOS
{
    public class ViewPostDTO
    {
        public String Title { get; set; } = string.Empty;
        public String Body { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set;  } = true;
        public string AuthorUserName { get; set; } = string.Empty;
        public string AuthorProfileLink { get; set; } = "www.google.com";
        public IFormFile? AuthorProfilePic { get; set; } = null;
        public int Likes { get; set; } = 0;
    }
    public static class PostExtensions
    {
        public static ViewPostDTO ToViewDTO(this Post post)
        {
            return new ViewPostDTO
            {
                Title = post.Title,
                Body = post.Body,
                CreatedDate = post.CreatedDate,
                IsPublic = post.IsPublic,
                AuthorUserName = post.Author?.UserName ?? "User Default",
                
            };
        }
    }
}
