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
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
