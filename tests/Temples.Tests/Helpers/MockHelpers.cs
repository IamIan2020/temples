using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Temples.Core.Entities;

namespace Temples.Tests.Helpers;

public static class MockHelpers
{
    public static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    public static Mock<SignInManager<ApplicationUser>> CreateMockSignInManager(
        Mock<UserManager<ApplicationUser>> userManager)
    {
        return new Mock<SignInManager<ApplicationUser>>(
            userManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            null!, null!, null!, null!);
    }

    public static Mock<RoleManager<ApplicationRole>> CreateMockRoleManager()
    {
        var store = new Mock<IRoleStore<ApplicationRole>>();
        return new Mock<RoleManager<ApplicationRole>>(
            store.Object, null!, null!, null!, null!);
    }

    public static IConfiguration CreateTestConfiguration()
    {
        var config = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "TestSuperSecretKeyAtLeast32Characters!!",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:AccessTokenExpirationMinutes"] = "15",
            ["Jwt:RefreshTokenExpirationDays"] = "7",
            ["FrontendUrl"] = "http://localhost:5173",
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }
}
