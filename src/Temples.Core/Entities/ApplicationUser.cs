using Microsoft.AspNetCore.Identity;

namespace Temples.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public string? ChineseName { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? MemberNumber { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
