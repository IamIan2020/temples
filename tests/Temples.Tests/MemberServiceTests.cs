using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Temples.Core.DTOs.Members;
using Temples.Core.Entities;
using Temples.Core.Services;
using Temples.Tests.Helpers;

namespace Temples.Tests;

public class MemberServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly MemberService _memberService;

    private readonly ApplicationUser _testUser = new()
    {
        Id = "user-1",
        Email = "test@example.com",
        UserName = "test@example.com",
        DisplayName = "測試使用者",
        ChineseName = "測試",
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

    public MemberServiceTests()
    {
        _userManagerMock = MockHelpers.CreateMockUserManager();
        _memberService = new MemberService(_userManagerMock.Object);
    }

    // --- GetProfile 測試 ---

    [Fact]
    public async Task GetProfileAsync_成功_回傳會員資料()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.GetRolesAsync(_testUser))
            .ReturnsAsync(new List<string> { "Member" });

        var result = await _memberService.GetProfileAsync("user-1");

        result.Should().NotBeNull();
        result.Id.Should().Be("user-1");
        result.Email.Should().Be("test@example.com");
        result.DisplayName.Should().Be("測試使用者");
        result.Roles.Should().Contain("Member");
    }

    [Fact]
    public async Task GetProfileAsync_找不到會員_拋出KeyNotFoundException()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("nonexist"))
            .ReturnsAsync((ApplicationUser?)null);

        var act = () => _memberService.GetProfileAsync("nonexist");

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("找不到該會員");
    }

    // --- UpdateProfile 測試 ---

    [Fact]
    public async Task UpdateProfileAsync_成功_回傳更新後的資料()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(_testUser))
            .ReturnsAsync(new List<string> { "Member" });

        var request = new UpdateProfileRequest
        {
            DisplayName = "新名稱",
            ChineseName = "新中文名",
            Gender = "男",
            Address = "台北市",
            Birthday = null
        };

        var result = await _memberService.UpdateProfileAsync("user-1", request);

        result.Should().NotBeNull();
        result.DisplayName.Should().Be("新名稱");
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u =>
            u.DisplayName == "新名稱" && u.ChineseName == "新中文名")), Times.Once);
    }

    // --- ChangePassword 測試 ---

    [Fact]
    public async Task ChangePasswordAsync_成功_不拋異常()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.ChangePasswordAsync(_testUser, "OldPass1!", "NewPass1!"))
            .ReturnsAsync(IdentityResult.Success);

        var act = () => _memberService.ChangePasswordAsync("user-1", new ChangePasswordRequest
        {
            CurrentPassword = "OldPass1!",
            NewPassword = "NewPass1!"
        });

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ChangePasswordAsync_舊密碼錯誤_拋出InvalidOperationException()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.ChangePasswordAsync(_testUser, "WrongOld!", "NewPass1!"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Incorrect password" }));

        var act = () => _memberService.ChangePasswordAsync("user-1", new ChangePasswordRequest
        {
            CurrentPassword = "WrongOld!",
            NewPassword = "NewPass1!"
        });

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    // --- ChangeRole 測試 ---

    [Fact]
    public async Task ChangeRoleAsync_成功_更新角色()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.GetRolesAsync(_testUser))
            .ReturnsAsync(new List<string> { "Member" });
        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(_testUser, It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(_testUser, "WebAdmin"))
            .ReturnsAsync(IdentityResult.Success);

        await _memberService.ChangeRoleAsync("user-1", "WebAdmin", "admin-1");

        _userManagerMock.Verify(x => x.AddToRoleAsync(_testUser, "WebAdmin"), Times.Once);
    }

    [Fact]
    public async Task ChangeRoleAsync_變更自己角色_拋出InvalidOperationException()
    {
        var act = () => _memberService.ChangeRoleAsync("user-1", "WebAdmin", "user-1");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("不可變更自己的角色");
    }

    // --- ChangeStatus 測試 ---

    [Fact]
    public async Task ChangeStatusAsync_停用會員_成功()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        await _memberService.ChangeStatusAsync("user-1", false, "admin-1");

        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => !u.IsActive)), Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_啟用會員_成功()
    {
        var inactiveUser = new ApplicationUser
        {
            Id = "user-2",
            Email = "inactive@example.com",
            DisplayName = "停用使用者",
            IsActive = false
        };

        _userManagerMock.Setup(x => x.FindByIdAsync("user-2"))
            .ReturnsAsync(inactiveUser);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        await _memberService.ChangeStatusAsync("user-2", true, "admin-1");

        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => u.IsActive)), Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_停用自己_拋出InvalidOperationException()
    {
        var act = () => _memberService.ChangeStatusAsync("admin-1", false, "admin-1");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("不可變更自己的帳號狀態");
    }

    // --- SoftDelete 測試 ---

    [Fact]
    public async Task SoftDeleteAsync_成功_將IsActive設為false()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        await _memberService.SoftDeleteAsync("user-1", "admin-1");

        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => !u.IsActive)), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteAsync_刪除自己_拋出InvalidOperationException()
    {
        var act = () => _memberService.SoftDeleteAsync("admin-1", "admin-1");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    // --- GetMemberById 測試 ---

    [Fact]
    public async Task GetMemberByIdAsync_成功_回傳會員資料()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(_testUser);
        _userManagerMock.Setup(x => x.GetRolesAsync(_testUser))
            .ReturnsAsync(new List<string> { "Member" });

        var result = await _memberService.GetMemberByIdAsync("user-1");

        result.Should().NotBeNull();
        result.Id.Should().Be("user-1");
    }

    [Fact]
    public async Task GetMemberByIdAsync_不存在_拋出KeyNotFoundException()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("nonexist"))
            .ReturnsAsync((ApplicationUser?)null);

        var act = () => _memberService.GetMemberByIdAsync("nonexist");

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
