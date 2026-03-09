using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 建立角色
        string[] roles = ["SystemAdmin", "WebAdmin", "Member"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // 建立預設系統管理員
        var adminUser = await userManager.FindByNameAsync("ianadmin");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "ianadmin",
                Email = "ianadmin@system.local",
                DisplayName = "系統管理員",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "my0919linda!");
            await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
        }
    }
}
