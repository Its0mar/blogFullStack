namespace ZeroBlog.Core.DTO;

public class AddFollowDTO
{
    public Guid FollowingId{ get; set; }
    public Guid FollowerId{ get; set; }
}