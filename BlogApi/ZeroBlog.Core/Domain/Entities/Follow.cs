using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.Domain.Entities;

public class Follow : BaseEntity
{
    public Guid Id { get; set; }
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }
    
    public ApplicationUser Follower { get; set; }
    public ApplicationUser Following { get; set; }  

}