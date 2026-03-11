using Microsoft.EntityFrameworkCore;
using Temples.Core.Entities;
using Temples.Core.Interfaces;

namespace Temples.Infrastructure.Data;

public class SystemSettingRepository : ISystemSettingRepository
{
    private readonly AppDbContext _context;

    public SystemSettingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SystemSetting> GetAsync()
    {
        return await _context.SystemSettings.FirstAsync(s => s.Id == 1);
    }

    public async Task UpdateAsync(SystemSetting setting)
    {
        setting.UpdatedAt = DateTime.UtcNow;
        _context.SystemSettings.Update(setting);
        await _context.SaveChangesAsync();
    }
}
