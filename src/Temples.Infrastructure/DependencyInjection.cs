using Microsoft.Extensions.DependencyInjection;
using Temples.Core.Interfaces;
using Temples.Core.Services;
using Temples.Infrastructure.Data;

namespace Temples.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
        services.AddScoped<ISystemSettingService, SystemSettingService>();
        return services;
    }
}
