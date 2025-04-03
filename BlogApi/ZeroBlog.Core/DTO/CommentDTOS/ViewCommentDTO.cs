
using System;
using Microsoft.AspNetCore.Http;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.DTO.CommentDTOS
{
    public class ViewCommentDTO
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public ApplicationUser Author { get; set; } 
        public IFormFile? ProfilePic { get; set; }
        public int LikesCount { get; set; } = 0;
        public int DislikesCount { get; set; } = 0;
        public bool IsLikedByCurrentUser { get; set; } = false;
        public bool IsDislikedByCurrentUser { get; set; } = false;


    }

    public static class CommentExtensions
    {
        public static ViewCommentDTO ToCommentDTO(this Comment comment)
        {
            ViewCommentDTO dto = new ViewCommentDTO
            {
                Content = comment.Content,
                CreatedAt = comment.CreatedDate,
                ParentCommentId = comment.ParentCommentId,
                AuthorName = comment.Author.PersonName,
                Author = comment.Author,
                LikesCount = comment.NumberOfLikes,
                DislikesCount = comment.NumberOfDisLikes,
            };

            return dto;
        }
    }
}
