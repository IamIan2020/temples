namespace Temples.Core.DTOs.Settings;

public class UpdateSystemSettingRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string WebsiteName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? TaxId { get; set; }
    public string Copyright { get; set; } = string.Empty;
    public int SessionTimeoutMinutes { get; set; }
}
