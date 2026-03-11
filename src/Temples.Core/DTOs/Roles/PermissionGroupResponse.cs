namespace Temples.Core.DTOs.Roles;

public class PermissionGroupResponse
{
    public string GroupName { get; set; } = string.Empty;
    public List<PermissionItem> Permissions { get; set; } = [];
}

public class PermissionItem
{
    public string Code { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
