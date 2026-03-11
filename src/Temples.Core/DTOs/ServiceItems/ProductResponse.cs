namespace Temples.Core.DTOs.ServiceItems;

public class ProductResponse
{
    public int Id { get; set; }
    public int ServiceItemId { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? PriceUnit { get; set; }
    public string? SubTitle { get; set; }
    public string? Description { get; set; }
    public int? MaxDonorCount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public int CurrentDonorCount { get; set; }
    public decimal CurrentTotalAmount { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
