using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IUserApiService
    {
        Task<SignInResponseModel> SignIn(SignInModel model);
        Task<SignInResponseModel> MicrosoftSignIn(string email);
    }
}