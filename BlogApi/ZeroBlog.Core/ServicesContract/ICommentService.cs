﻿
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.DTO.CommentDTOS;

namespace ZeroBlog.Core.ServicesContract
{
    public interface ICommentService
    {
        public Task<bool> AddCommentAsync(AddCommentDTO dto, Guid userId);
        public Task<IEnumerable<Comment>> GetAllCommentsAsync();
    }
}
