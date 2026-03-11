using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.DTOs;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public UploadController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost("image")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            var url = await _fileUploadService.UploadImageAsync(file);
            return Ok(ApiResponse<object>.Ok(new { url }));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }
}
