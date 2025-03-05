
using ZeroBlog.Core.Domain.IdentityEntities;
using ZeroBlog.Core.DTO;

namespace ZeroBlog.Core.ServicesContract
{
    public interface IAccountService
    {
        public Task<bool> UpdateAccountInfoAsync(ApplicationUser user, UpdateAccInfoDTO dto);

    }
}
