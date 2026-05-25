using AuthAPI.DTOs;

namespace AuthAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<object>> ActivateAccount(LoginDto loginDto);
        Task<LoginResponse> Login(LoginDto loginDto);
        Task<LoginResponse> MicrosoftLogin(MicrosoftSignInRequest request);
        Task<ApiResponse<AuthResponse>> RefreshToken(RefreshTokenDto refreshTokenDto);
        Task<ApiResponse<object>> SignOut(string? email);
    }
}