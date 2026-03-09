using Microsoft.Extensions.DependencyInjection;
using Temples.Core.Interfaces;
using Temples.Core.Services;

namespace Temples.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
