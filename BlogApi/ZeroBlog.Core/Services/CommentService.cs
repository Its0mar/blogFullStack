using System.Linq.Expressions;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.RepositoryContracts;
using ZeroBlog.Core.DTO.CommentDTOS;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _repo;
        private readonly IPostService _postService;
        public CommentService(IGenericRepository<Comment> repo, IPostService postService)
        {
            _repo = repo;
            _postService = postService;
        }

        public async Task<bool> AddCommentAsync(AddCommentDTO dto, Guid userId)
        {
            Comment? parent= null;
            if (dto.ParentCommentId != null)
                parent = await _repo.GetByIdAsync(dto.ParentCommentId.Value);


            var post = await _postService.GetPostAsync(dto.PostId);
            
            if (post == null || (dto.ParentCommentId != null && parent == null))
            {
                return false;
            }

            var newComment = new Comment
            {
                Content = dto.Content,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                AuthorId = userId,
                PostID = dto.PostId,
                ParentCommentId = dto.ParentCommentId,
            };

            await _repo.AddAsync(newComment);
            return true;

        }


        public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await _repo.GetByIdAsync(commentId);
            if (comment == null || comment.AuthorId != userId)
            {
                return false;
            }
            await _repo.DeleteAsync(comment);
            return true;
        }


        public async Task<IEnumerable<ViewCommentDTO>> GetAllCommentsAsyncForPost(Guid postId)
        {
            var comments = await _repo.GetWithIncludeAsync(comment => comment.PostID == postId, new Expression<Func<Comment, Object>>[] { comment => comment.Author });
            var dtos =  comments.Select(c => c.ToCommentDTO()).ToList();
            return dtos;
        }
        public async Task<IEnumerable<ViewCommentDTO>> GetAllCommentsAsyncForUser(Guid userId)
        {
            var comments = await _repo.GetWithIncludeAsync(comment => comment.AuthorId == userId, new Expression<Func<Comment, Object>>[] { comment => comment.Author });
            var dtos = comments.Select(c => c.ToCommentDTO()).ToList();
            return dtos;
        }
    }
}
