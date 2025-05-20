using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Api.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class FollowController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFollowService _followService;

    public FollowController(UserManager<ApplicationUser> userManager, IFollowService followService)
    {
        _userManager = userManager;
        _followService = followService;
    }
    [HttpPost]
    public async Task<IActionResult> AddFollowAsync([FromBody] AddFollowDTO dto)
    {
        // to do : check if both user exist
        var result = await _followService.AddFollowAsync(dto);
        
        if (result)
            return Ok();
        
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetFollowers()
    {
        var f = await _followService.GetAllFollowersAsync(getCurrentUserId());
        return Ok(f);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFollowing()
    {
        var f = await _followService.GetAllFollowingAsync(getCurrentUserId());
        return Ok(f);
    }
    
    private Guid getCurrentUserId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return (id == null ? Guid.Empty : Guid.Parse(id));
    }
    
}