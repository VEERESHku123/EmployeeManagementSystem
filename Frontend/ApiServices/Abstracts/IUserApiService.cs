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
    }
}