using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroBlog.Core.DTO.CommentDTOS;
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
                return Problem(statusCode: 500, title: "Failed to create a comment", detail: "Error while creating comment");

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId, getCurrentUserId());
            if (result == false)
                return Problem(statusCode: 500, title: "Failed to delete a comment", detail: "Error while deleting comment");

            return Ok();
        }

        private Guid getCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return id == null ? Guid.Empty : Guid.Parse(id);
        }
    }
}
