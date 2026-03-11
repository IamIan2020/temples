using Microsoft.EntityFrameworkCore;
using Temples.Core.Entities;
using Temples.Core.Interfaces;

namespace Temples.Infrastructure.Data;

public class ServiceItemRepository : IServiceItemRepository
{
    private readonly AppDbContext _context;

    public ServiceItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceItem>> GetAllAsync()
    {
        return await _context.ServiceItems
            .Include(s => s.Options.OrderBy(o => o.SortOrder))
            .OrderBy(s => s.SortOrder)
            .ToListAsync();
    }

    public async Task<ServiceItem?> GetByIdAsync(int id)
    {
        return await _context.ServiceItems
            .Include(s => s.Options.OrderBy(o => o.SortOrder))
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<ServiceItem> CreateAsync(ServiceItem item)
    {
        _context.ServiceItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(ServiceItem item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ServiceItem item)
    {
        _context.ServiceItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ServiceItem>> GetActiveListAsync()
    {
        return await _context.ServiceItems
            .Where(s => s.IsActive)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();
    }

    public async Task<ServiceItem?> GetActiveByIdAsync(int id)
    {
        return await _context.ServiceItems
            .Include(s => s.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
            .Where(s => s.IsActive)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
