namespace Temples.Core.DTOs.Members;

public class UpdateProfileRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string? ChineseName { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
}
