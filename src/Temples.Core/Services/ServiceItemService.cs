using Temples.Core.DTOs.ServiceItems;
using Temples.Core.Entities;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class ServiceItemService : IServiceItemService
{
    private readonly IServiceItemRepository _repository;

    public ServiceItemService(IServiceItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ServiceItemResponse>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(MapToResponse).ToList();
    }

    public async Task<ServiceItemResponse?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item == null ? null : MapToResponse(item);
    }

    public async Task<ServiceItemResponse> CreateAsync(CreateServiceItemRequest request)
    {
        var item = new ServiceItem
        {
            HeaderImage = request.HeaderImage,
            Title = request.Title,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive,
            HtmlContent = request.HtmlContent,
            Options = request.Options.Select(o => new ServiceItemOption
            {
                Title = o.Title,
                Price = o.Price,
                PriceUnit = o.PriceUnit,
                SubTitle = o.SubTitle,
                Description = o.Description,
                MaxDonorCount = o.MaxDonorCount,
                MaxTotalAmount = o.MaxTotalAmount,
                SortOrder = o.SortOrder,
                IsActive = o.IsActive,
                StartDate = ToUtc(o.StartDate),
                EndDate = ToUtc(o.EndDate),
            }).ToList()
        };

        await _repository.CreateAsync(item);
        return MapToResponse(item);
    }

    public async Task<ServiceItemResponse?> UpdateAsync(int id, UpdateServiceItemRequest request)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) return null;

        item.HeaderImage = request.HeaderImage;
        item.Title = request.Title;
        item.SortOrder = request.SortOrder;
        item.IsActive = request.IsActive;
        item.HtmlContent = request.HtmlContent;

        // Options 差異更新（只處理未刪除的）
        var activeOptions = item.Options.Where(o => !o.IsDeleted).ToList();
        var existingOptionIds = activeOptions.Select(o => o.Id).ToHashSet();
        var requestOptionIds = request.Options.Where(o => o.Id.HasValue).Select(o => o.Id!.Value).ToHashSet();

        // 軟刪除不在列表中的選項
        var toDelete = activeOptions.Where(o => !requestOptionIds.Contains(o.Id)).ToList();
        foreach (var opt in toDelete)
        {
            opt.IsDeleted = true;
        }

        // 更新或新增
        foreach (var reqOpt in request.Options)
        {
            if (reqOpt.Id.HasValue && existingOptionIds.Contains(reqOpt.Id.Value))
            {
                // 更新現有選項
                var existing = item.Options.First(o => o.Id == reqOpt.Id.Value);
                existing.Title = reqOpt.Title;
                existing.Price = reqOpt.Price;
                existing.PriceUnit = reqOpt.PriceUnit;
                existing.SubTitle = reqOpt.SubTitle;
                existing.Description = reqOpt.Description;
                existing.MaxDonorCount = reqOpt.MaxDonorCount;
                existing.MaxTotalAmount = reqOpt.MaxTotalAmount;
                existing.SortOrder = reqOpt.SortOrder;
                existing.IsActive = reqOpt.IsActive;
                existing.StartDate = ToUtc(reqOpt.StartDate);
                existing.EndDate = ToUtc(reqOpt.EndDate);
            }
            else
            {
                // 新增選項
                item.Options.Add(new ServiceItemOption
                {
                    Title = reqOpt.Title,
                    Price = reqOpt.Price,
                    PriceUnit = reqOpt.PriceUnit,
                    SubTitle = reqOpt.SubTitle,
                    Description = reqOpt.Description,
                    MaxDonorCount = reqOpt.MaxDonorCount,
                    MaxTotalAmount = reqOpt.MaxTotalAmount,
                    SortOrder = reqOpt.SortOrder,
                    IsActive = reqOpt.IsActive,
                    StartDate = ToUtc(reqOpt.StartDate),
                    EndDate = ToUtc(reqOpt.EndDate),
                });
            }
        }

        await _repository.UpdateAsync(item);
        return MapToResponse(item);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) return false;
        await _repository.DeleteAsync(item);
        return true;
    }

    public async Task UpdateSortOrderAsync(UpdateSortOrderRequest request)
    {
        var items = await _repository.GetAllAsync();
        foreach (var sortItem in request.Items)
        {
            var item = items.FirstOrDefault(i => i.Id == sortItem.Id);
            if (item != null) item.SortOrder = sortItem.SortOrder;
        }
        await _repository.SaveChangesAsync();
    }

    public async Task<List<PublicServiceItemResponse>> GetActiveListAsync()
    {
        var items = await _repository.GetActiveListAsync();
        return items.Select(i => new PublicServiceItemResponse
        {
            Id = i.Id,
            HeaderImage = i.HeaderImage,
            Title = i.Title,
            SortOrder = i.SortOrder,
        }).ToList();
    }

    public async Task<PublicServiceItemDetailResponse?> GetActiveByIdAsync(int id)
    {
        var item = await _repository.GetActiveByIdAsync(id);
        if (item == null) return null;

        return new PublicServiceItemDetailResponse
        {
            Id = item.Id,
            HeaderImage = item.HeaderImage,
            Title = item.Title,
            HtmlContent = item.HtmlContent,
            Options = item.Options
                .Where(o => !o.IsDeleted && o.IsActive)
                .Where(o => !o.StartDate.HasValue || o.StartDate.Value <= DateTime.UtcNow)
                .Where(o => !o.EndDate.HasValue || o.EndDate.Value >= DateTime.UtcNow)
                .Select(o => new PublicServiceItemOptionResponse
                {
                    Id = o.Id,
                    Title = o.Title,
                    Price = o.Price,
                    PriceUnit = o.PriceUnit,
                    SubTitle = o.SubTitle,
                    Description = o.Description,
                    MaxDonorCount = o.MaxDonorCount,
                    MaxTotalAmount = o.MaxTotalAmount,
                    CurrentDonorCount = o.CurrentDonorCount,
                    CurrentTotalAmount = o.CurrentTotalAmount,
                }).ToList()
        };
    }

    // 商品 CRUD
    public async Task<List<ProductResponse>> GetAllProductsAsync()
    {
        var options = await _repository.GetAllProductsAsync();
        return options.Select(MapToProductResponse).ToList();
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        var option = await _repository.GetProductByIdAsync(id);
        return option == null ? null : MapToProductResponse(option);
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var option = new ServiceItemOption
        {
            ServiceItemId = request.ServiceItemId,
            Title = request.Title,
            Price = request.Price,
            PriceUnit = request.PriceUnit,
            SubTitle = request.SubTitle,
            Description = request.Description,
            MaxDonorCount = request.MaxDonorCount,
            MaxTotalAmount = request.MaxTotalAmount,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive,
            StartDate = ToUtc(request.StartDate),
            EndDate = ToUtc(request.EndDate),
        };

        var created = await _repository.CreateProductAsync(option);
        return MapToProductResponse(created);
    }

    public async Task<ProductResponse?> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var option = await _repository.GetProductByIdAsync(id);
        if (option == null) return null;

        option.ServiceItemId = request.ServiceItemId;
        option.Title = request.Title;
        option.Price = request.Price;
        option.PriceUnit = request.PriceUnit;
        option.SubTitle = request.SubTitle;
        option.Description = request.Description;
        option.MaxDonorCount = request.MaxDonorCount;
        option.MaxTotalAmount = request.MaxTotalAmount;
        option.SortOrder = request.SortOrder;
        option.IsActive = request.IsActive;
        option.StartDate = ToUtc(request.StartDate);
        option.EndDate = ToUtc(request.EndDate);

        await _repository.UpdateProductAsync(option);
        // reload to get updated ServiceItem navigation
        return await GetProductByIdAsync(id);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var option = await _repository.GetProductByIdAsync(id);
        if (option == null) return false;
        option.IsDeleted = true;
        await _repository.UpdateProductAsync(option);
        return true;
    }

    private static ProductResponse MapToProductResponse(ServiceItemOption o) => new()
    {
        Id = o.Id,
        ServiceItemId = o.ServiceItemId,
        CategoryTitle = o.ServiceItem?.Title ?? "",
        Title = o.Title,
        Price = o.Price,
        PriceUnit = o.PriceUnit,
        SubTitle = o.SubTitle,
        Description = o.Description,
        MaxDonorCount = o.MaxDonorCount,
        MaxTotalAmount = o.MaxTotalAmount,
        CurrentDonorCount = o.CurrentDonorCount,
        CurrentTotalAmount = o.CurrentTotalAmount,
        SortOrder = o.SortOrder,
        IsActive = o.IsActive,
        StartDate = o.StartDate,
        EndDate = o.EndDate,
    };

    private static DateTime? ToUtc(DateTime? dt)
    {
        if (dt == null) return null;
        return dt.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc)
            : dt.Value.ToUniversalTime();
    }

    private static ServiceItemResponse MapToResponse(ServiceItem item) => new()
    {
        Id = item.Id,
        HeaderImage = item.HeaderImage,
        Title = item.Title,
        SortOrder = item.SortOrder,
        IsActive = item.IsActive,
        HtmlContent = item.HtmlContent,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
        Options = item.Options.Where(o => !o.IsDeleted).Select(o => new ServiceItemOptionResponse
        {
            Id = o.Id,
            Title = o.Title,
            Price = o.Price,
            PriceUnit = o.PriceUnit,
            SubTitle = o.SubTitle,
            Description = o.Description,
            MaxDonorCount = o.MaxDonorCount,
            MaxTotalAmount = o.MaxTotalAmount,
            CurrentDonorCount = o.CurrentDonorCount,
            CurrentTotalAmount = o.CurrentTotalAmount,
            SortOrder = o.SortOrder,
            IsActive = o.IsActive,
            StartDate = o.StartDate,
            EndDate = o.EndDate,
        }).ToList()
    };
}
