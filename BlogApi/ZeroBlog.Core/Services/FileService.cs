
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ZeroBlog.Core.ServicesContract;

namespace ZeroBlog.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string[] allowedExt, int sizeLimitInMB, string folder)
        {
            var ext = Path.GetExtension(file.FileName);
            if (!allowedExt.Contains(ext))
            {
                throw new ArgumentException($"Invalid Type, The extension must be {string.Join(',', allowedExt)}");
            }
            if (file.Length > (sizeLimitInMB * 1024 * 1024))
            {
                throw new ArgumentException($"size must not be more than {sizeLimitInMB} MB");
            }
            var newFileName = Path.GetRandomFileName() + ext;
            var folderPath = Path.Combine(_environment.WebRootPath, folder);
            var filePath = Path.Combine(folderPath, newFileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) 
                    return false;
                File.Delete(filePath);
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
