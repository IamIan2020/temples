using Temples.Core.DTOs.ServiceItems;

namespace Temples.Core.Interfaces;

public interface IServiceItemService
{
    Task<List<ServiceItemResponse>> GetAllAsync();
    Task<ServiceItemResponse?> GetByIdAsync(int id);
    Task<ServiceItemResponse> CreateAsync(CreateServiceItemRequest request);
    Task<ServiceItemResponse?> UpdateAsync(int id, UpdateServiceItemRequest request);
    Task<bool> DeleteAsync(int id);
    Task UpdateSortOrderAsync(UpdateSortOrderRequest request);
    Task<List<PublicServiceItemResponse>> GetActiveListAsync();
    Task<PublicServiceItemDetailResponse?> GetActiveByIdAsync(int id);
}
