
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Core.ServicesContract
{
    public interface IJwtService
    {
        public string CreateJwtToken(ApplicationUser user);
    }
}
