namespace Temples.Core.DTOs.ServiceItems;

public class CreateServiceItemRequest
{
    public string? HeaderImage { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? HtmlContent { get; set; }
    public List<CreateServiceItemOptionRequest> Options { get; set; } = [];
}
