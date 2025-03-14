using ZeroBlog.Core.DTO.PostDTOS;
using ZeroBlog.Core.ServicesContract;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.RepositoryContracts;

namespace ZeroBlog.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _repo;
        public PostService(IGenericRepository<Post> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<Post?> GetPostAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task AddPostAsync(AddPostDTO dto)
        {
            var post = dto.ToPost();
            await _repo.AddAsync(post);
        }

        public async Task<bool> DeletePostAsync(Guid postID)
        {
            var post = await _repo.GetByIdAsync(postID);
            if (post == null)
            {
                return false;
            }

            await _repo.DeleteAsync(post);
            return true;
        }
    }
}
