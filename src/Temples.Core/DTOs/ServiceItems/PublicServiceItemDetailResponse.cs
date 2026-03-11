namespace Temples.Core.DTOs.ServiceItems;

public class PublicServiceItemDetailResponse
{
    public int Id { get; set; }
    public string? HeaderImage { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? HtmlContent { get; set; }
    public List<PublicServiceItemOptionResponse> Options { get; set; } = [];
}

public class PublicServiceItemOptionResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? PriceUnit { get; set; }
    public string? SubTitle { get; set; }
    public string? Description { get; set; }
    public int? MaxDonorCount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public int CurrentDonorCount { get; set; }
    public decimal CurrentTotalAmount { get; set; }
}
