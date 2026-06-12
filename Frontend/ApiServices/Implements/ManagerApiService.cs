using Frontend.ApiServices.Abstracts;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using Microsoft.Extensions.Caching.Memory;

namespace Frontend.ApiServices.Implements
{
    public class ManagerApiService : BaseApiService, IManagerApiService
    {
        private readonly IMemoryCache cache;
        public ManagerApiService(IMemoryCache cache, IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend")
        {
            this.cache = cache;

        }

        public async Task<ApiResponse<List<ManagerModel>>> SendAllManagers()
        {
            try
            {
                if (cache.TryGetValue("Managers",out ApiResponse<List<ManagerModel>> cachedManagers))
                {
                    return cachedManagers;
                }

                var response = await SendAuthorizedRequestAsync(() => client.GetAsync("manager/all"));

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<ManagerModel>>
                    {
                        Success = false,
                        Message = "Failed to fetch managers"
                    };
                }

                var result = await response.Content
                                            .ReadFromJsonAsync<ApiResponse<List<ManagerModel>>>()
                                            ?? new ApiResponse<List<ManagerModel>>
                                            {
                                                Success = false,
                                                Message = "No response"
                                            };

                if (result.Success)
                {
                    cache.Set("Managers",result,TimeSpan.FromHours(1));
                }

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
