using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.DTOs;
using Temples.Core.DTOs.Auth;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IValidator<RegisterRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(ApiResponse<LoginResponse>.Ok(result, "註冊成功"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IValidator<LoginRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(ApiResponse<LoginResponse>.Ok(result, "登入成功"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(ApiResponse<LoginResponse>.Ok(result));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        [FromServices] IValidator<ForgotPasswordRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
        await _authService.ForgotPasswordAsync(request.Email, frontendUrl);

        return Ok(ApiResponse.Ok("若該 Email 已註冊，將收到重設密碼信件"));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        [FromServices] IValidator<ResetPasswordRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            await _authService.ResetPasswordAsync(request);
            return Ok(ApiResponse.Ok("密碼重設成功"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }
}
