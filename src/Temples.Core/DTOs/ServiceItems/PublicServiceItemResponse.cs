namespace Temples.Core.DTOs.ServiceItems;

public class PublicServiceItemResponse
{
    public int Id { get; set; }
    public string? HeaderImage { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
