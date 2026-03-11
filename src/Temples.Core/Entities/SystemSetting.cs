namespace Temples.Core.Entities;

public class SystemSetting
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = "宮廟系統";
    public string WebsiteName { get; set; } = "宮廟系統";
    public string? Phone { get; set; }
    public string? TaxId { get; set; }
    public string Copyright { get; set; } = "© 2026 宮廟系統";
    public int SessionTimeoutMinutes { get; set; } = 30;
    public string? Address { get; set; }
    public string? Fax { get; set; }
    public string? LineUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? GoogleMapUrl { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
