namespace Temples.Core.DTOs.Members;

public class MemberProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? ChineseName { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? MemberNumber { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = [];
}
