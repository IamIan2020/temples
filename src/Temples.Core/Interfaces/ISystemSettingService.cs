using Temples.Core.DTOs.Settings;

namespace Temples.Core.Interfaces;

public interface ISystemSettingService
{
    Task<SystemSettingResponse> GetSettingsAsync();
    Task<PublicSettingResponse> GetPublicSettingsAsync();
    Task<SystemSettingResponse> UpdateSettingsAsync(UpdateSystemSettingRequest request);
}
