using Frontend.ApiServices.Interfaces;
using Frontend.Models;

namespace Frontend.ApiServices.Implements
{
    public class UserApiService :  BaseApiService, IUserApiService
    {
        public UserApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Auth") { }
        

        public async Task<SignInResponseModel> SignIn(SignInModel model)
        {
            try
            {
                var response = await client.PostAsJsonAsync("login", model);

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

        public async Task<ApiResponse<object>> ActivateAccount(ActivateAccountModel model)
        {
            try
            {
                var url = client.BaseAddress + "activateAccount";

                var response = await client.PostAsJsonAsync(url, model);

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

                return result!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ApiResponse<object>> SignOut()
        {
            var response =
                await SendAuthorizedRequestAsync(
                    () => client.PostAsync("signOut", null));

            if (response == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to sign out"
                };
            }

            return await response
                .Content
                .ReadFromJsonAsync<ApiResponse<object>>()

                ?? new ApiResponse<object>
                {
                    Success = true,
                    Message = "Signed out successfully"
                };
        }

    }
}
