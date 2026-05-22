using Frontend.ApiServices.Interfaces;
using Frontend.Models;

namespace Frontend.ApiServices.Implements
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient client;

        public UserApiService(IHttpClientFactory factory)
        {
            client = factory.CreateClient("Auth");
        }

        public async Task<SignInResponseModel> SignIn(SignInModel model)
        {
            try
            {
                var response = await client.PostAsJsonAsync("login", model);

                if (!response.IsSuccessStatusCode)
                    return null;

                var result = await response.Content.ReadFromJsonAsync<SignInResponseModel>();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SignInResponseModel> MicrosoftSignIn(string email)
        {
            try
            {
                var url = client.BaseAddress + "microsoft-signin";
                var response = await client.PostAsJsonAsync(url, new
                {
                    Email = email
                });

                var result = await response.Content.ReadFromJsonAsync<SignInResponseModel>();
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
