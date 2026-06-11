using Frontend.Models.Common;
using Frontend.Models.User;

namespace Frontend.ApiServices.Abstracts
{
    public interface IUserApiService
    {
        Task<SignInResponseModel> SignIn(SignInModel model);
        Task<SignInResponseModel> MicrosoftSignIn(string email);
        Task<ApiResponse<object>> ActivateAccount(ActivateAccountModel model);
        Task<ApiResponse<object>> SignOut();

        Task<ApiResponse<object>> ForgotPasswordAsync(string email);

        Task<ApiResponse<string>> VerifyOtpAsync(string email, string otp);

        Task<ApiResponse<object>> ResetPasswordAsync(string resetToken, string newPassword);
    }
}