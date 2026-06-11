using AuthAPI.DTOs.Common;
using AuthAPI.DTOs.SigIn;

namespace AuthAPI.Services.Abstracts
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