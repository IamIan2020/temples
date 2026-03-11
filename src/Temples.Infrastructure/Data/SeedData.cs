using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Temples.Core.Constants;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        // 建立內建角色
        var builtInRoles = new[]
        {
            ("SystemAdmin", "系統管理員 - 最高權限"),
            ("WebAdmin", "網站管理員 - 管理功能"),
            ("Member", "會員 - 標準使用者"),
        };

        foreach (var (roleName, description) in builtInRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                    Description = description,
                    IsBuiltIn = true
                });
            }
            else
            {
                // 確保現有角色有 IsBuiltIn 標記
                var existingRole = await roleManager.FindByNameAsync(roleName);
                if (existingRole != null && !existingRole.IsBuiltIn)
                {
                    existingRole.IsBuiltIn = true;
                    existingRole.Description = description;
                    await roleManager.UpdateAsync(existingRole);
                }
            }
        }

        // 設定 WebAdmin 預設權限
        var webAdminRole = await roleManager.FindByNameAsync("WebAdmin");
        if (webAdminRole != null)
        {
            var existingClaims = await roleManager.GetClaimsAsync(webAdminRole);
            var webAdminPermissions = new[]
            {
                Permissions.MembersView,
                Permissions.MembersEdit,
                Permissions.MembersDelete,
            };

            foreach (var permission in webAdminPermissions)
            {
                if (!existingClaims.Any(c => c.Type == "permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(webAdminRole, new Claim("permission", permission));
                }
            }
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
