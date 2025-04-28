using ZeroBlog.Core.DTO;

namespace ZeroBlog.Core.ServicesContract;

public interface IFollowService
{
    public Task<bool> AddFollowAsync(AddFollowDTO dto);
    public Task<IEnumerable<string?>> GetAllFollowingAsync(Guid userId);
    public Task<IEnumerable<string?>> GetAllFollowersAsync(Guid userId);
}