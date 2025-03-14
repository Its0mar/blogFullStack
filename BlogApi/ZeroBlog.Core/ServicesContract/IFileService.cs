using Microsoft.AspNetCore.Http;

namespace ZeroBlog.Core.ServicesContract
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file, string[] allowedExt, int sizeLimitInMB, string folder);
        public bool DeleteFile(string filePath);


    }
}
