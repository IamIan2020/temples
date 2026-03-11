using Temples.Core.DTOs.Settings;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class SystemSettingService : ISystemSettingService
{
    private readonly ISystemSettingRepository _repository;

    public SystemSettingService(ISystemSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemSettingResponse> GetSettingsAsync()
    {
        var setting = await _repository.GetAsync();
        return new SystemSettingResponse
        {
            Id = setting.Id,
            CompanyName = setting.CompanyName,
            WebsiteName = setting.WebsiteName,
            Phone = setting.Phone,
            TaxId = setting.TaxId,
            Copyright = setting.Copyright,
            SessionTimeoutMinutes = setting.SessionTimeoutMinutes,
            UpdatedAt = setting.UpdatedAt
        };
    }

    public async Task<PublicSettingResponse> GetPublicSettingsAsync()
    {
        var setting = await _repository.GetAsync();
        return new PublicSettingResponse
        {
            WebsiteName = setting.WebsiteName,
            Copyright = setting.Copyright,
            SessionTimeoutMinutes = setting.SessionTimeoutMinutes
        };
    }

    public async Task<SystemSettingResponse> UpdateSettingsAsync(UpdateSystemSettingRequest request)
    {
        var setting = await _repository.GetAsync();
        setting.CompanyName = request.CompanyName;
        setting.WebsiteName = request.WebsiteName;
        setting.Phone = request.Phone;
        setting.TaxId = request.TaxId;
        setting.Copyright = request.Copyright;
        setting.SessionTimeoutMinutes = request.SessionTimeoutMinutes;
        await _repository.UpdateAsync(setting);

        return new SystemSettingResponse
        {
            Id = setting.Id,
            CompanyName = setting.CompanyName,
            WebsiteName = setting.WebsiteName,
            Phone = setting.Phone,
            TaxId = setting.TaxId,
            Copyright = setting.Copyright,
            SessionTimeoutMinutes = setting.SessionTimeoutMinutes,
            UpdatedAt = setting.UpdatedAt
        };
    }
}
