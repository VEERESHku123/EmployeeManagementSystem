using Frontend.Exceptions;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Abstracts
{
    public class BaseApiService
    {
        protected readonly HttpClient client;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly IHttpClientFactory factory;

        protected BaseApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor, string clientName)
        {
            this.factory = factory;
            client = factory.CreateClient(clientName);
            this.httpContextAccessor = httpContextAccessor;
        }

        protected async Task<HttpResponseMessage?> SendAuthorizedRequestAsync(Func<Task<HttpResponseMessage>> request)
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await request();

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            var newToken = await RefreshAccessToken();

            if (string.IsNullOrEmpty(newToken))
            {
                throw new SessionExpiredException();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

            return await request();
        }
        private async Task<string?> RefreshAccessToken()
        {

            var refreshToken = httpContextAccessor.HttpContext?.Session.GetString("RefreshToken");


            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var authClient = factory.CreateClient("Auth");

            var response = await authClient.PostAsJsonAsync("user/refresh-token", new { RefreshToken = refreshToken });

            if (!response.IsSuccessStatusCode)
            {
                httpContextAccessor.HttpContext?.Session.Clear();

                httpContextAccessor.HttpContext?.Response.Cookies.Delete(".AspNetCore.Session");

                await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

            if (result == null || !result.Success)
            {
                httpContextAccessor.HttpContext?.Session.Clear();

                httpContextAccessor.HttpContext?.Response.Cookies.Delete(".AspNetCore.Session");

                await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return null;
            }

            httpContextAccessor.HttpContext?.Session.SetString("AccessToken", result.Data.Token);

            httpContextAccessor.HttpContext?.Session.SetString("RefreshToken", result.Data.RefreshToken);

            return result.Data.Token;
        }
    }
}
