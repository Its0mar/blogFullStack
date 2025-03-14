using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroBlog.Core.DTO.CommentDTOS;
using ZeroBlog.Core.Services;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] AddCommentDTO dto)
        {
            var result = await _commentService.AddCommentAsync(dto, getCurrentUserId());
            if (result == false)
                return BadRequest();
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            return Ok(await _commentService.GetAllCommentsAsync());
        }
        private Guid getCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return (id == null ? Guid.Empty : Guid.Parse(id));
        }
    }
}
