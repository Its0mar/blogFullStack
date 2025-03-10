using ZeroBlog.Core.DTO.PostDTOS;
using ZeroBlog.Core.Domain;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.ServicesContract;

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
