
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.DTO.CommentDTOS;

namespace ZeroBlog.Core.ServicesContract
{
    public interface ICommentService
    {
        public Task<bool> AddCommentAsync(AddCommentDTO dto, Guid userId);
        public    Task<bool> DeleteCommentAsync(Guid commentId, Guid userId);
        public Task<IEnumerable<ViewCommentDTO>> GetAllCommentsAsyncForPost(Guid postId);
        public Task<IEnumerable<ViewCommentDTO>> GetAllCommentsAsyncForUser(Guid userId);
    }
}
