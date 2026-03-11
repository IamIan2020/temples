namespace Temples.Core.DTOs.ServiceItems;

public class UpdateSortOrderRequest
{
    public List<SortOrderItem> Items { get; set; } = [];
}

public class SortOrderItem
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
}
