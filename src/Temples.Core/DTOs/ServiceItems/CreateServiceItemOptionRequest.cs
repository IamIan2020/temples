namespace Temples.Core.DTOs.ServiceItems;

public class CreateServiceItemOptionRequest
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? PriceUnit { get; set; }
    public string? SubTitle { get; set; }
    public string? Description { get; set; }
    public int? MaxDonorCount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
