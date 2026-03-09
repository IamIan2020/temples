using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Temples.Core.DTOs.Members;
using Temples.Core.Entities;
using Temples.Core.Interfaces;

namespace Temples.Core.Services;

public class MemberService : IMemberService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public MemberService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<MemberProfileResponse> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("找不到該會員");

        return await MapToResponse(user);
    }

    public async Task<MemberProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("找不到該會員");

        user.DisplayName = request.DisplayName;
        user.ChineseName = request.ChineseName;
        user.Birthday = request.Birthday;
        user.Gender = request.Gender;
        user.Address = request.Address;
        user.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return await MapToResponse(user);
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("找不到該會員");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new InvalidOperationException(string.Join("; ", errors));
        }
    }

    public async Task<MemberListResponse> GetMemberListAsync(MemberListRequest request)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(u =>
                u.Email!.ToLower().Contains(search) ||
                u.DisplayName.ToLower().Contains(search) ||
                (u.ChineseName != null && u.ChineseName.ToLower().Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var items = new List<MemberProfileResponse>();
        foreach (var user in users)
        {
            items.Add(await MapToResponse(user));
        }

        return new MemberListResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<MemberProfileResponse> GetMemberByIdAsync(string memberId)
    {
        var user = await _userManager.FindByIdAsync(memberId)
            ?? throw new KeyNotFoundException("找不到該會員");

        return await MapToResponse(user);
    }

    public async Task ChangeRoleAsync(string memberId, string newRole, string currentUserId)
    {
        if (memberId == currentUserId)
            throw new InvalidOperationException("不可變更自己的角色");

        var user = await _userManager.FindByIdAsync(memberId)
            ?? throw new KeyNotFoundException("找不到該會員");

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);
    }

    public async Task ChangeStatusAsync(string memberId, bool isActive, string currentUserId)
    {
        if (memberId == currentUserId)
            throw new InvalidOperationException("不可變更自己的帳號狀態");

        var user = await _userManager.FindByIdAsync(memberId)
            ?? throw new KeyNotFoundException("找不到該會員");

        user.IsActive = isActive;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
    }

    public async Task SoftDeleteAsync(string memberId, string currentUserId)
    {
        await ChangeStatusAsync(memberId, false, currentUserId);
    }

    private async Task<MemberProfileResponse> MapToResponse(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new MemberProfileResponse
        {
            Id = user.Id,
            Email = user.Email!,
            DisplayName = user.DisplayName,
            ChineseName = user.ChineseName,
            Birthday = user.Birthday,
            Gender = user.Gender,
            Address = user.Address,
            MemberNumber = user.MemberNumber,
            JoinDate = user.JoinDate,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            Roles = roles.ToList()
        };
    }
}
