using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroBlog.Core.DTO.PostDTOS;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ViewPostDTO>>> GetAllPostsAsync()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts.ToList());
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<ViewPostDTO>>> GetAllUserPublicPostsAsync(Guid userId)
        {
            var posts = await _postService.GetAllUserPublicPostAsync(userId);
            return Ok(posts.ToList());
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<ViewPostDTO>>> GetAllUserPostsAsync(Guid userId)
        {
            var posts = await _postService.GetAllUserPostAsync(userId);
            return Ok(posts.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddPostAsync([FromBody] AddPostDTO dto)
        {
            dto.AuthorId = getCurrentUserId();
            if (dto.AuthorId == Guid.Empty)
                return BadRequest();
            await _postService.AddPostAsync(dto);
            return Ok();

        }

        [HttpDelete]
        public async Task<IActionResult> DeletePostAsync([FromBody]Guid postID)
        {
            var result = await _postService.DeletePostAsync(postID);
            if (!result)
                return NotFound();

            return Ok();
        }



        private Guid getCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return (id == null ? Guid.Empty : Guid.Parse(id));
        }
    }
}
