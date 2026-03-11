namespace Temples.Core.Entities;

public class ServiceItem
{
    public int Id { get; set; }
    public string? HeaderImage { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? HtmlContent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<ServiceItemOption> Options { get; set; } = [];
}
