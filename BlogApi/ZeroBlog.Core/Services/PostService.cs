using ZeroBlog.Core.DTO.PostDTOS;
using ZeroBlog.Core.ServicesContract;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using ZeroBlog.Core.Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore;

namespace ZeroBlog.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostService(IGenericRepository<Post> repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
       
        public async Task<IEnumerable<ViewPostDTO>> GetAllPostsAsync()
        {
            var posts =  await _repo.GetWithIncludeAsync(post => true, new Expression<Func<Post, Object>>[] { post => post.Author });
            var viewPosts = posts.Select(p => p.ToViewDTO());
            return viewPosts;
        }

        public async Task<IEnumerable<ViewPostDTO>> GetAllUserPublicPostAsync(Guid userID)
        {
            // if there no user with this id then return empty list
            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user == null)
                return new List<ViewPostDTO>();

            var posts = await _repo.GetWithIncludeAsync(post => post.AuthorId == userID && post.IsPublic, new Expression<Func<Post, Object>>[] { post => post.Author });
            var viewPosts = posts.Select(p => p.ToViewDTO());
            return viewPosts;
        }
        public async Task<IEnumerable<ViewPostDTO>> GetAllUserPostAsync(Guid userID)
        {
            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user == null)
                return new List<ViewPostDTO>();

            var posts = await _repo.GetWithIncludeAsync(post => post.AuthorId == userID, new Expression<Func<Post, Object>>[] { post => post.Author });
            var viewPosts = posts.Select(p => p.ToViewDTO());
            return viewPosts;
        }

        public async Task<Post?> GetPostAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task AddPostAsync(AddPostDTO dto)
        {
            var post = dto.ToPost();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == dto.AuthorId);
            if (user is null)
                return;
            post.Author = user;
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
