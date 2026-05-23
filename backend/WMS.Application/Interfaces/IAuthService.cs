using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync(int userId);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
