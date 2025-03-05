using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroBlog.Core.ServicesContract
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file, string[] allowedExt, int sizeLimitInMB, string folder);
        public bool DeleteFile(string filePath);


    }
}
