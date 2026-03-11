using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

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
            var result = await userManager.CreateAsync(adminUser, "My0919linda!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
            }
        }

        // 建立預設系統設定
        if (!await dbContext.SystemSettings.AnyAsync())
        {
            dbContext.SystemSettings.Add(new SystemSetting
            {
                Id = 1,
                CompanyName = "宮廟系統",
                WebsiteName = "宮廟系統",
                Copyright = "© 2026 宮廟系統",
                SessionTimeoutMinutes = 30,
                UpdatedAt = DateTime.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
