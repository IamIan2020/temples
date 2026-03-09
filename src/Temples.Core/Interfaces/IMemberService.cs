using Temples.Core.DTOs.Members;

namespace Temples.Core.Interfaces;

public interface IMemberService
{
    Task<MemberProfileResponse> GetProfileAsync(string userId);
    Task<MemberProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<MemberListResponse> GetMemberListAsync(MemberListRequest request);
    Task<MemberProfileResponse> GetMemberByIdAsync(string memberId);
    Task ChangeRoleAsync(string memberId, string newRole, string currentUserId);
    Task ChangeStatusAsync(string memberId, bool isActive, string currentUserId);
    Task SoftDeleteAsync(string memberId, string currentUserId);
}
