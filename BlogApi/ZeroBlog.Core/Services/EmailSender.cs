using DotNetEnv;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PostmarkDotNet;


namespace ZeroBlog.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string? _apiKey;

        public EmailSender(IConfiguration configuration)
        {
            Env.Load();
            _apiKey = Environment.GetEnvironmentVariable("SendGridApi");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var message = new PostmarkMessage()
            {
                To = email,
                From = "armadillo23886@mailshan.com",
                TrackOpens = true,
                Subject = subject,
                HtmlBody = htmlMessage,
            };

            try
            {
                var client = new PostmarkClient(Environment.GetEnvironmentVariable("EmailSenderApi"));
                var sendResult = await client.SendMessageAsync(message);
                
            }
            catch (Exception)
            {
                throw;
            }



            
        }

    }
}
