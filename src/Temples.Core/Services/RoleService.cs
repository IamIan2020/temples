using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Temples.Core.Constants;
using Temples.Core.DTOs.Roles;
using Temples.Core.Entities;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<RoleResponse>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles.ToList();
        var result = new List<RoleResponse>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            result.Add(new RoleResponse
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
                IsBuiltIn = role.IsBuiltIn,
                Permissions = claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value)
                    .ToList()
            });
        }

        return result;
    }

    public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId)
            ?? throw new KeyNotFoundException("找不到該角色");

        var claims = await _roleManager.GetClaimsAsync(role);
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name!,
            Description = role.Description,
            IsBuiltIn = role.IsBuiltIn,
            Permissions = claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList()
        };
    }

    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
    {
        var existingRole = await _roleManager.FindByNameAsync(request.Name);
        if (existingRole != null)
            throw new InvalidOperationException("角色名稱已存在");

        var role = new ApplicationRole
        {
            Name = request.Name,
            Description = request.Description,
            IsBuiltIn = false
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            throw new InvalidOperationException($"建立角色失敗：{string.Join(", ", result.Errors.Select(e => e.Description))}");

        // 新增權限 claims
        foreach (var permission in request.Permissions)
        {
            if (Permissions.All.Contains(permission))
            {
                await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }
        }

        return await GetRoleByIdAsync(role.Id);
    }

    public async Task<RoleResponse> UpdateRoleAsync(string roleId, UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId)
            ?? throw new KeyNotFoundException("找不到該角色");

        // 內建角色不可修改名稱
        if (role.IsBuiltIn && role.Name != request.Name)
            throw new InvalidOperationException("內建角色名稱不可修改");

        // 檢查新名稱是否與其他角色重複
        if (role.Name != request.Name)
        {
            var existingRole = await _roleManager.FindByNameAsync(request.Name);
            if (existingRole != null)
                throw new InvalidOperationException("角色名稱已存在");
        }

        role.Name = request.Name;
        role.Description = request.Description;
        await _roleManager.UpdateAsync(role);

        // 同步權限 claims：先刪再加
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in existingClaims.Where(c => c.Type == "permission"))
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var permission in request.Permissions)
        {
            if (Permissions.All.Contains(permission))
            {
                await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }
        }

        return await GetRoleByIdAsync(role.Id);
    }

    public async Task DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId)
            ?? throw new KeyNotFoundException("找不到該角色");

        if (role.IsBuiltIn)
            throw new InvalidOperationException("內建角色不可刪除");

        // 檢查是否有使用者使用此角色
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        if (usersInRole.Any())
            throw new InvalidOperationException($"該角色仍有 {usersInRole.Count} 位使用者，無法刪除");

        await _roleManager.DeleteAsync(role);
    }

    public Task<List<PermissionGroupResponse>> GetAllPermissionsAsync()
    {
        var groups = Permissions.Groups.Select(g => new PermissionGroupResponse
        {
            GroupName = g.Key,
            Permissions = g.Value.Select(p => new PermissionItem
            {
                Code = p.Code,
                DisplayName = p.DisplayName
            }).ToList()
        }).ToList();

        return Task.FromResult(groups);
    }
}
