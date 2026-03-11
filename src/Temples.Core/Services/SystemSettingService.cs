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
            Address = setting.Address,
            Fax = setting.Fax,
            LineUrl = setting.LineUrl,
            FacebookUrl = setting.FacebookUrl,
            GoogleMapUrl = setting.GoogleMapUrl,
            LogoUrl = setting.LogoUrl,
            UpdatedAt = setting.UpdatedAt
        };
    }

    public async Task<PublicSettingResponse> GetPublicSettingsAsync()
    {
        var setting = await _repository.GetAsync();
        return new PublicSettingResponse
        {
            CompanyName = setting.CompanyName,
            WebsiteName = setting.WebsiteName,
            Phone = setting.Phone,
            Copyright = setting.Copyright,
            SessionTimeoutMinutes = setting.SessionTimeoutMinutes,
            Address = setting.Address,
            Fax = setting.Fax,
            LineUrl = setting.LineUrl,
            FacebookUrl = setting.FacebookUrl,
            GoogleMapUrl = setting.GoogleMapUrl,
            LogoUrl = setting.LogoUrl
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
        setting.Address = request.Address;
        setting.Fax = request.Fax;
        setting.LineUrl = request.LineUrl;
        setting.FacebookUrl = request.FacebookUrl;
        setting.GoogleMapUrl = request.GoogleMapUrl;
        setting.LogoUrl = request.LogoUrl;
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
            Address = setting.Address,
            Fax = setting.Fax,
            LineUrl = setting.LineUrl,
            FacebookUrl = setting.FacebookUrl,
            GoogleMapUrl = setting.GoogleMapUrl,
            LogoUrl = setting.LogoUrl,
            UpdatedAt = setting.UpdatedAt
        };
    }
}
