using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Temples.Core.DTOs.Auth;
using Temples.Core.Entities;
using Temples.Core.Interfaces;
using Temples.Core.Services;
using Temples.Tests.Helpers;
using System.Security.Claims;

namespace Temples.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userManagerMock = MockHelpers.CreateMockUserManager();
        _signInManagerMock = MockHelpers.CreateMockSignInManager(_userManagerMock);
        _roleManagerMock = MockHelpers.CreateMockRoleManager();
        _emailServiceMock = new Mock<IEmailService>();
        var config = MockHelpers.CreateTestConfiguration();

        // 預設：任何角色名稱都回傳一個 ApplicationRole，且無 claims
        _roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => new ApplicationRole { Name = name });
        _roleManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationRole>()))
            .ReturnsAsync(new List<Claim>());

        _authService = new AuthService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            config,
            _emailServiceMock.Object);
    }

    // --- 註冊測試 ---

    [Fact]
    public async Task RegisterAsync_成功_回傳LoginResponse()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Test1234!",
            ConfirmPassword = "Test1234!",
            DisplayName = "測試使用者"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Member"))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "Member" });

        var result = await _authService.RegisterAsync(request);

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be(request.Email);
        result.User.DisplayName.Should().Be(request.DisplayName);
        result.User.Roles.Should().Contain("Member");
    }

    [Fact]
    public async Task RegisterAsync_Email已被使用_拋出InvalidOperationException()
    {
        var request = new RegisterRequest
        {
            Email = "existing@example.com",
            Password = "Test1234!",
            ConfirmPassword = "Test1234!",
            DisplayName = "測試"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "該 Email 已被註冊" }));

        var act = () => _authService.RegisterAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*已被註冊*");
    }

    // --- 登入測試 ---

    [Fact]
    public async Task LoginAsync_成功_回傳LoginResponse()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            UserName = "test@example.com",
            DisplayName = "測試使用者",
            IsActive = true
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, "Test1234!", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Member" });

        var result = await _authService.LoginAsync(new LoginRequest
        {
            Email = "test@example.com",
            Password = "Test1234!"
        });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.User.Id.Should().Be("user-1");
    }

    [Fact]
    public async Task LoginAsync_Email不存在_拋出UnauthorizedAccessException()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync("nonexist@example.com"))
            .ReturnsAsync((ApplicationUser?)null);

        var act = () => _authService.LoginAsync(new LoginRequest
        {
            Email = "nonexist@example.com",
            Password = "Test1234!"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("帳號或密碼錯誤");
    }

    [Fact]
    public async Task LoginAsync_密碼錯誤_拋出UnauthorizedAccessException()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            DisplayName = "測試",
            IsActive = true
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, "WrongPass!", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var act = () => _authService.LoginAsync(new LoginRequest
        {
            Email = "test@example.com",
            Password = "WrongPass!"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("帳號或密碼錯誤");
    }

    [Fact]
    public async Task LoginAsync_帳號停用_拋出UnauthorizedAccessException()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            DisplayName = "測試",
            IsActive = false
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        var act = () => _authService.LoginAsync(new LoginRequest
        {
            Email = "test@example.com",
            Password = "Test1234!"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("帳號已停用");
    }

    // --- 忘記密碼測試 ---

    [Fact]
    public async Task ForgotPasswordAsync_Email存在_寄送重設信()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            DisplayName = "測試"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("reset-token-123");

        await _authService.ForgotPasswordAsync("test@example.com", "http://localhost:5173");

        _emailServiceMock.Verify(
            x => x.SendPasswordResetEmailAsync("test@example.com", It.Is<string>(s => s.Contains("reset-token-123"))),
            Times.Once);
    }

    [Fact]
    public async Task ForgotPasswordAsync_Email不存在_不寄信不拋異常()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync("nonexist@example.com"))
            .ReturnsAsync((ApplicationUser?)null);

        await _authService.ForgotPasswordAsync("nonexist@example.com", "http://localhost:5173");

        _emailServiceMock.Verify(
            x => x.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    // --- 重設密碼測試 ---

    [Fact]
    public async Task ResetPasswordAsync_成功_不拋異常()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            DisplayName = "測試"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "token", "NewPass1234!"))
            .ReturnsAsync(IdentityResult.Success);

        var act = () => _authService.ResetPasswordAsync(new ResetPasswordRequest
        {
            Email = "test@example.com",
            Token = "token",
            NewPassword = "NewPass1234!"
        });

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ResetPasswordAsync_Token無效_拋出InvalidOperationException()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            DisplayName = "測試"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "bad-token", "NewPass1234!"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

        var act = () => _authService.ResetPasswordAsync(new ResetPasswordRequest
        {
            Email = "test@example.com",
            Token = "bad-token",
            NewPassword = "NewPass1234!"
        });

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ResetPasswordAsync_Email不存在_拋出InvalidOperationException()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync("nonexist@example.com"))
            .ReturnsAsync((ApplicationUser?)null);

        var act = () => _authService.ResetPasswordAsync(new ResetPasswordRequest
        {
            Email = "nonexist@example.com",
            Token = "token",
            NewPassword = "NewPass1234!"
        });

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("無效的重設密碼請求");
    }

    // --- Token 刷新測試 ---

    [Fact]
    public async Task RefreshTokenAsync_成功_回傳新Token()
    {
        // 先註冊取得合法 refresh token
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "test@example.com",
            UserName = "test@example.com",
            DisplayName = "測試",
            IsActive = true
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "Test1234!"))
            .Callback<ApplicationUser, string>((u, _) => u.Id = "user-1")
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Member"))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "Member" });
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        var registerResult = await _authService.RegisterAsync(new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Test1234!",
            ConfirmPassword = "Test1234!",
            DisplayName = "測試"
        });

        var result = await _authService.RefreshTokenAsync(registerResult.RefreshToken);

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.User.Id.Should().Be("user-1");
    }

    [Fact]
    public async Task RefreshTokenAsync_無效Token_拋出例外()
    {
        var act = () => _authService.RefreshTokenAsync("invalid-token");

        await act.Should().ThrowAsync<Exception>();
    }
}
