using Frontend.Models;

namespace Frontend.APIs
{
    public class ManagerAPI
    {
        private readonly HttpClient client;

        public ManagerAPI(IHttpClientFactory factory)
        {
            client = factory.CreateClient("BackEnd");
        }

        public async Task<(List<ManagerModel> managersList, int StatusCode)> SendAllManagers()
        {
            try
            {
                var response = await client.GetAsync("manager/all");

                if (!response.IsSuccessStatusCode)
                {
                    return (new List<ManagerModel>(), (int)response.StatusCode);
                }

                var managers = await response.Content.ReadFromJsonAsync<List<ManagerModel>>();

                return (managers ?? new List<ManagerModel>(), (int)response.StatusCode);
            }
            catch
            {
                throw;
            }
        }
    }
}
