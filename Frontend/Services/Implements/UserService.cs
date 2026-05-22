using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Frontend.Services.Interfaces;

namespace Frontend.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserApiService userApiService;

        public UserService(IUserApiService userApiService)
        {
            this.userApiService = userApiService;
        }

        public async Task<SignInResponseModel> SignIn(SignInModel model)
        {
            return await userApiService.SignIn(model);
        }

        public async Task<SignInResponseModel> MicrosoftSignIn(string email)
        {
            return await userApiService.MicrosoftSignIn(email);
        }
    }
}
