using Frontend.Models;

namespace Frontend.APIs
{
    public class AccountApi
    {
        private readonly HttpClient client;

        public AccountApi(IHttpClientFactory factory)
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

            return await response.Content.ReadFromJsonAsync<AuthResponse>();

            
        }
    }
}
