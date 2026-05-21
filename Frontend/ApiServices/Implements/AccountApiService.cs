using Frontend.ApiServices.Interfaces;
using Frontend.Models;

namespace Frontend.ApiServices.Implements
{
    public class AccountApiService : IAccountApiService
    {
        private readonly HttpClient client;

        public AccountApiService(IHttpClientFactory factory)
        {
            client = factory.CreateClient("Auth");
        }

        public async Task<AuthResponse> GetJwtToken(string email, string name)
        {
            var url = client.BaseAddress + "microsoft-signin";
            var response = await client.PostAsJsonAsync(url, new
            {
                Email = email,
                Name = name
            });

            if (!response.IsSuccessStatusCode)
                return null;

            Console.WriteLine("------------------------------------------");
            Console.WriteLine(response.StatusCode);

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
    }
}
