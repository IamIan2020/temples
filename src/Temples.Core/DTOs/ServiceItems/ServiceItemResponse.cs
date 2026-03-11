namespace Temples.Core.DTOs.ServiceItems;

public class ServiceItemResponse
{
    public int Id { get; set; }
    public string? HeaderImage { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? HtmlContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ServiceItemOptionResponse> Options { get; set; } = [];
}
