using Frontend.Models;

namespace Frontend.Services.Interfaces
{
    public interface IUserService
    {
        Task<SignInResponseModel> SignIn(SignInModel model);
        Task<SignInResponseModel> MicrosoftSignIn(string email);
    }
}