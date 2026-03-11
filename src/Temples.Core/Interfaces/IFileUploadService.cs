using Microsoft.AspNetCore.Http;

namespace Temples.Core.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string subfolder = "images");
}
