using System.Linq.Expressions;
using ZeroBlog.Core.Domain.Entities;
using ZeroBlog.Core.Domain.RepositoryContracts;
using ZeroBlog.Core.DTO;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Core.Services;

public class FollowService : IFollowService
{
    private readonly IGenericRepository<Follow> _repo;

    public FollowService(IGenericRepository<Follow> repo)
    {
        _repo = repo;
    }

    public async Task<bool> AddFollowAsync(AddFollowDTO dto)
    {
        
        var isFollowed = await IsFollowedAsync(dto.FollowerId, dto.FollowingId);
        // case : the user already followed
        if (isFollowed)
            return false;
        
        await _repo.AddAsync(new Follow { FollowerId = dto.FollowerId, FollowingId = dto.FollowingId });
        return true;
        
        
    }

    public async Task<IEnumerable<string?>> GetAllFollowersAsync(Guid userId)
    {
        var list = await _repo.GetWithIncludeAsync(f => f.FollowingId == userId , new Expression<Func<Follow, object>>[] {follow => follow.Follower});
        IEnumerable<string?> names = new List<string?>();
        names = list.Select(f => f.Follower.UserName);

        return names;
    } 
    
    public async Task<IEnumerable<string?>> GetAllFollowingAsync(Guid userId)
    {
        var list = await _repo.GetWithIncludeAsync(f=> f.FollowerId == userId , new Expression<Func<Follow, Object>>[] { follow => follow.Following });
        IEnumerable<string?> names = new List<string?>();
        names = list.Select(f => f.Following.UserName);

        return names;
    } 
    private async Task<bool> IsFollowedAsync(Guid followerId, Guid followingId)
    {
        return await _repo.IsExistAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
    }
}