using Temples.Core.Entities;

namespace Temples.Core.Interfaces;

public interface IServiceItemRepository
{
    Task<List<ServiceItem>> GetAllAsync();
    Task<ServiceItem?> GetByIdAsync(int id);
    Task<ServiceItem> CreateAsync(ServiceItem item);
    Task UpdateAsync(ServiceItem item);
    Task DeleteAsync(ServiceItem item);
    Task<List<ServiceItem>> GetActiveListAsync();
    Task<ServiceItem?> GetActiveByIdAsync(int id);
    Task SaveChangesAsync();

    // 商品 CRUD
    Task<List<ServiceItemOption>> GetAllProductsAsync();
    Task<ServiceItemOption?> GetProductByIdAsync(int id);
    Task<ServiceItemOption> CreateProductAsync(ServiceItemOption option);
    Task UpdateProductAsync(ServiceItemOption option);
}
