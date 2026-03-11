using Temples.Core.DTOs.Roles;

namespace Temples.Core.Interfaces;

public interface IRoleService
{
    Task<List<RoleResponse>> GetAllRolesAsync();
    Task<RoleResponse> GetRoleByIdAsync(string roleId);
    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleResponse> UpdateRoleAsync(string roleId, UpdateRoleRequest request);
    Task DeleteRoleAsync(string roleId);
    Task<List<PermissionGroupResponse>> GetAllPermissionsAsync();
}
