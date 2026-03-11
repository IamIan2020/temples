namespace Temples.Core.DTOs.Settings;

public class PublicSettingResponse
{
    public string WebsiteName { get; set; } = string.Empty;
    public string Copyright { get; set; } = string.Empty;
    public int SessionTimeoutMinutes { get; set; }
}
