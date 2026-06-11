using AuthAPI.DTOs.Common;

namespace AuthAPI.Services.Abstracts
{
    public interface IPasswordResetService
    {
        Task<ApiResponse<object>> ForgotPasswordAsync(string email);
        Task<ApiResponse<object>> ResetPasswordAsync(string resetToken, string newPassword);
        Task<ApiResponse<string>> VerifyOtpAsync(string email, string otp);
    }
}