using Microsoft.AspNetCore.Identity;

namespace Temples.Core.Entities;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public bool IsBuiltIn { get; set; }
}
