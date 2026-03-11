using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Temples.Core.Constants;
using Temples.Core.DTOs;
using Temples.Core.DTOs.Roles;
using Temples.Core.Interfaces;

namespace Temples.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = Permissions.RolesManage)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        return Ok(ApiResponse<List<RoleResponse>>.Ok(result));
    }

    [HttpGet("permissions")]
    public async Task<IActionResult> GetAllPermissions()
    {
        var result = await _roleService.GetAllPermissionsAsync();
        return Ok(ApiResponse<List<PermissionGroupResponse>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        try
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            return Ok(ApiResponse<RoleResponse>.Ok(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該角色"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(
        [FromBody] CreateRoleRequest request,
        [FromServices] IValidator<CreateRoleRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _roleService.CreateRoleAsync(request);
            return Created($"/api/roles/{result.Id}", ApiResponse<RoleResponse>.Ok(result, "角色建立成功"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(
        string id,
        [FromBody] UpdateRoleRequest request,
        [FromServices] IValidator<UpdateRoleRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse.Fail("驗證失敗", validation.Errors.Select(e => e.ErrorMessage).ToList()));

        try
        {
            var result = await _roleService.UpdateRoleAsync(id, request);
            return Ok(ApiResponse<RoleResponse>.Ok(result, "角色更新成功"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該角色"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        try
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(ApiResponse.Ok("角色已刪除"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse.Fail("找不到該角色"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }
}
