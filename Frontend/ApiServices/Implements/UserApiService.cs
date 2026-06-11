using Frontend.ApiServices.Abstracts;
using Frontend.Models.Common;
using Frontend.Models.User;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Implements
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)  
        {
            client = factory.CreateClient("Auth");
            this.httpContextAccessor = httpContextAccessor;
        }
        

        public async Task<SignInResponseModel> SignIn(SignInModel model)
        {
            try
            {
                var response = await client.PostAsJsonAsync("user/signIn", model);

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
                var url = client.BaseAddress + "user/microsoft-signin";
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
                var url = client.BaseAddress + "user/activateAccount";

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
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await client.PostAsync("user/signOut", null);


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


        // Forget Password 
        public async Task<ApiResponse<object>> ForgotPasswordAsync(string email)
        {
            var response = await client.PostAsJsonAsync("password-reset/forgot-password",
                new
                {
                    Email = email
                });

            return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        }

        public async Task<ApiResponse<string>> VerifyOtpAsync(string email, string otp)
        {
            var response = await client.PostAsJsonAsync("password-reset/verify-otp",
                new
                {
                    Email = email,
                    Otp = otp
                });

            return await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(string resetToken, string newPassword)
        {
            var response = await client.PostAsJsonAsync("password-reset/reset-password",
                new
                {
                    ResetToken = resetToken,
                    NewPassword = newPassword
                });

            return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        }
    }
}
