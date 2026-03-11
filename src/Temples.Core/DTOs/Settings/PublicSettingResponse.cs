namespace Temples.Core.DTOs.Settings;

public class PublicSettingResponse
{
    public string CompanyName { get; set; } = string.Empty;
    public string WebsiteName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Copyright { get; set; } = string.Empty;
    public int SessionTimeoutMinutes { get; set; }
    public string? Address { get; set; }
    public string? Fax { get; set; }
    public string? LineUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? GoogleMapUrl { get; set; }
    public string? LogoUrl { get; set; }
}
