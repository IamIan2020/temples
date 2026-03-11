using Temples.Core.DTOs.ServiceItems;

namespace Temples.Core.Interfaces;

public interface IServiceItemService
{
    // 分類
    Task<List<ServiceItemResponse>> GetAllAsync();
    Task<ServiceItemResponse?> GetByIdAsync(int id);
    Task<ServiceItemResponse> CreateAsync(CreateServiceItemRequest request);
    Task<ServiceItemResponse?> UpdateAsync(int id, UpdateServiceItemRequest request);
    Task<bool> DeleteAsync(int id);
    Task UpdateSortOrderAsync(UpdateSortOrderRequest request);
    Task<List<PublicServiceItemResponse>> GetActiveListAsync();
    Task<PublicServiceItemDetailResponse?> GetActiveByIdAsync(int id);

    // 商品
    Task<List<ProductResponse>> GetAllProductsAsync();
    Task<ProductResponse?> GetProductByIdAsync(int id);
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<ProductResponse?> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteProductAsync(int id);
}
