﻿
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.DTO.PostDTOS;

namespace ZeroBlog.Core.ServicesContract
{
    public interface IPostService
    {
        public Task<IEnumerable<Post>> GetAllPostsAsync();
        public Task AddPostAsync(AddPostDTO dto);
        public Task<bool> DeletePostAsync(Guid postID);
        public Task<Post?> GetPostAsync(Guid id);
    }
}
