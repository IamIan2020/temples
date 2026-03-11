namespace Temples.Core.Entities;

public class ServiceItemOption
{
    public int Id { get; set; }
    public int ServiceItemId { get; set; }
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
    public bool IsActive { get; set; } = true;

    public ServiceItem ServiceItem { get; set; } = null!;
}
