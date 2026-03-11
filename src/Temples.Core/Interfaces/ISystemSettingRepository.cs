using Temples.Core.Entities;

namespace Temples.Core.Interfaces;

public interface ISystemSettingRepository
{
    Task<SystemSetting> GetAsync();
    Task UpdateAsync(SystemSetting setting);
}
