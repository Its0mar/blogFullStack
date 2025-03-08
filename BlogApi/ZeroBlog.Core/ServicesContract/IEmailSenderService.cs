

namespace ZeroBlog.Core.ServicesContract
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
