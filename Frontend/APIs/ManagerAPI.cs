using Frontend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Frontend.APIs
{
    public class ManagerAPI
    {
        private readonly HttpClient client;
        private readonly IMemoryCache cache;

        public ManagerAPI(IHttpClientFactory factory, IMemoryCache cache)
        {
            client = factory.CreateClient("BackEnd");
            this.cache = cache;
        }

        public async Task<List<ManagerModel>> SendAllManagers()
        {
            try
            {
                if(!cache.TryGetValue("Managers", out List<ManagerModel> managers))
                {
                    managers = await client.GetFromJsonAsync<List<ManagerModel>>("manager/all");

                    cache.Set(
                        "Managers",
                        managers,
                        TimeSpan.FromHours(1));
                }

                return managers?? new List<ManagerModel>();
            }
            catch
            {
                throw;
            }
        }
    }
}
