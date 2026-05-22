using AuthAPI.DTOs;

namespace AuthAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool success, string message)> ActivateAccount(LoginDto loginDto);
        Task<LoginResponse> Login(LoginDto loginDto);
        Task<LoginResponse> MicrosoftLogin(MicrosoftSignInRequest request);
    }
}