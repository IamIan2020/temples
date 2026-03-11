using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _env;
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

    public FileUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string subfolder = "images")
    {
        if (file.Length == 0)
            throw new ArgumentException("檔案不可為空");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("檔案大小不得超過 10MB");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException("僅支援 jpg、png、gif、webp 格式");

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{subfolder}/{fileName}";
    }

    public void CleanupUnusedImages(string? oldHtml, string? newHtml)
    {
        var oldUrls = ExtractImageUrls(oldHtml);
        var newUrls = ExtractImageUrls(newHtml);

        var removed = oldUrls.Except(newUrls);
        foreach (var url in removed)
        {
            // 只刪除 /uploads/ 開頭的本站圖片
            if (!url.StartsWith("/uploads/")) continue;
            var filePath = Path.Combine(_env.WebRootPath, url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
            {
                try { File.Delete(filePath); } catch { /* 忽略刪除失敗 */ }
            }
        }
    }

    private static HashSet<string> ExtractImageUrls(string? html)
    {
        if (string.IsNullOrEmpty(html)) return [];
        var matches = Regex.Matches(html, @"<img[^>]+src=""([^""]+)""", RegexOptions.IgnoreCase);
        return matches.Select(m => m.Groups[1].Value).ToHashSet();
    }
}
