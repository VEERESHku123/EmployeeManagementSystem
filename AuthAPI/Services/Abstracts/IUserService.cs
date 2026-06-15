using AuthAPI.DTOs.Common;
using AuthAPI.DTOs.SigIn;

namespace AuthAPI.Services.Abstracts
{
    public interface IUserService
    {
        Task<ApiResponse<object>> ActivateAccount(ActivateAccountDto activateAccountDto);
        Task<SignInResponse> SignIn(SignInDto loginDto);
        Task<SignInResponse> MicrosoftLogin(MicrosoftSignInRequest request);
        Task<ApiResponse<AuthResponse>> RefreshToken(RefreshTokenDto refreshTokenDto);
        Task<ApiResponse<object>> SignOut(string? email);
    }
}