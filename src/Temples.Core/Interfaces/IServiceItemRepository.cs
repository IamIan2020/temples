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
}
