using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.DTOs;
using Temples.Core.DTOs.Members;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException("無法取得使用者 ID");

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await _memberService.GetProfileAsync(GetUserId());
        return Ok(ApiResponse<MemberProfileResponse>.Ok(result));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] UpdateProfileRequest request,
        [FromServices] IValidator<UpdateProfileRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        var result = await _memberService.UpdateProfileAsync(GetUserId(), request);
        return Ok(ApiResponse<MemberProfileResponse>.Ok(result, "更新成功"));
    }

    [HttpPut("me/password")]
    public async Task<IActionResult> ChangeMyPassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] IValidator<ChangePasswordRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            await _memberService.ChangePasswordAsync(GetUserId(), request);
            return Ok(ApiResponse.Ok("密碼變更成功"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpGet]
    [Authorize(Roles = "SystemAdmin,WebAdmin")]
    public async Task<IActionResult> GetMemberList([FromQuery] MemberListRequest request)
    {
        var result = await _memberService.GetMemberListAsync(request);
        return Ok(ApiResponse<MemberListResponse>.Ok(result));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SystemAdmin,WebAdmin")]
    public async Task<IActionResult> GetMemberById(string id)
    {
        try
        {
            var result = await _memberService.GetMemberByIdAsync(id);
            return Ok(ApiResponse<MemberProfileResponse>.Ok(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該會員"));
        }
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> ChangeRole(
        string id,
        [FromBody] ChangeRoleRequest request,
        [FromServices] IValidator<ChangeRoleRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            await _memberService.ChangeRoleAsync(id, request.Role, GetUserId());
            return Ok(ApiResponse.Ok("角色變更成功"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該會員"));
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "SystemAdmin,WebAdmin")]
    public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeStatusRequest request)
    {
        try
        {
            await _memberService.ChangeStatusAsync(id, request.IsActive, GetUserId());
            return Ok(ApiResponse.Ok(request.IsActive ? "帳號已啟用" : "帳號已停用"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該會員"));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SystemAdmin,WebAdmin")]
    public async Task<IActionResult> SoftDelete(string id)
    {
        try
        {
            await _memberService.SoftDeleteAsync(id, GetUserId());
            return Ok(ApiResponse.Ok("會員已停用"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該會員"));
        }
    }
}
