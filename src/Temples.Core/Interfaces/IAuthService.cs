using Temples.Core.DTOs.Auth;

namespace Temples.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task ForgotPasswordAsync(string email, string frontendUrl);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}
