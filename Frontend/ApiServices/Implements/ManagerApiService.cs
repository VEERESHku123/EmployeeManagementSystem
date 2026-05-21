using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Implements
{
    public class ManagerApiService : IManagerApiService
    {
        private readonly HttpClient client;
        private readonly IMemoryCache cache;
        private readonly IHttpContextAccessor context;
        public ManagerApiService(IHttpClientFactory factory, IMemoryCache cache, IHttpContextAccessor context)
        {
            client = factory.CreateClient("BackEnd");
            this.cache = cache;
            this.context = context;
        }

        public async Task<List<ManagerModel>> SendAllManagers()
        {
            try
            {
                if (!cache.TryGetValue("Managers", out List<ManagerModel> managers))
                {
                    var token = context.HttpContext?.Session.GetString("JwtToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    managers = await client.GetFromJsonAsync<List<ManagerModel>>("manager/all");

                    cache.Set(
                        "Managers",
                        managers,
                        TimeSpan.FromHours(1));
                }

                return managers ?? new List<ManagerModel>();
            }
            catch
            {
                throw;
            }
        }
    }
}
