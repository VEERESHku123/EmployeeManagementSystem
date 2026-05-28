using Frontend.Models.Common;
using Frontend.Models.Employee;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Interfaces
{
    public class BaseApiService
    {
        protected readonly HttpClient client;
        protected readonly IHttpContextAccessor httpContextAccessor;

        protected BaseApiService(IHttpClientFactory factory,IHttpContextAccessor httpContextAccessor, string clientName)
        {
            client = factory.CreateClient(clientName);
            this.httpContextAccessor = httpContextAccessor;
        }

        protected async Task<HttpResponseMessage?> SendAuthorizedRequestAsync(Func<Task<HttpResponseMessage>> request)
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await request();

            // Access token expired
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newToken = await RefreshAccessToken();

                if (string.IsNullOrEmpty(newToken))
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                response = await request();
            }

            return response;
        }

        private async Task<string?> RefreshAccessToken()
        {
            var refreshToken = httpContextAccessor.HttpContext?.Session.GetString("RefreshToken");

            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var response = await client.PostAsJsonAsync("user/refresh-token",
                    new
                    {
                        RefreshToken = refreshToken
                    });

            if (!response.IsSuccessStatusCode)
            {
                httpContextAccessor.HttpContext?.Session.Clear();

                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

            if (result == null || !result.Success)
            {
                return null;
            }

            httpContextAccessor.HttpContext?.Session.SetString("AccessToken", result.Data.Token);

            httpContextAccessor.HttpContext?.Session.SetString("RefreshToken", result.Data.RefreshToken);

            return result.Data.Token;
        }
    }
}
