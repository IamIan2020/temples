using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.DTOs;
using Temples.Core.DTOs.Settings;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/system-settings")]
public class SystemSettingsController : ControllerBase
{
    private readonly ISystemSettingService _settingService;

    public SystemSettingsController(ISystemSettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicSettings()
    {
        var result = await _settingService.GetPublicSettingsAsync();
        return Ok(ApiResponse<PublicSettingResponse>.Ok(result));
    }

    [HttpGet]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> GetSettings()
    {
        var result = await _settingService.GetSettingsAsync();
        return Ok(ApiResponse<SystemSettingResponse>.Ok(result));
    }

    [HttpPut]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> UpdateSettings(
        [FromBody] UpdateSystemSettingRequest request,
        [FromServices] IValidator<UpdateSystemSettingRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        var result = await _settingService.UpdateSettingsAsync(request);
        return Ok(ApiResponse<SystemSettingResponse>.Ok(result, "設定已更新"));
    }
}
