
using Microsoft.AspNetCore.Http;

namespace ZeroBlog.Core.DTO
{
    public class AuthorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IFormFile? ProfilePic { get; set; }
        public string? Bio { get; set; }
        public int NumOfPosts { get; set; } = 0;
        public int Following { get; set; } = 0;
        public int Followers { get; set; } = 0;
    }
}
