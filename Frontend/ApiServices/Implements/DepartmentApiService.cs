using Frontend.ApiServices.Interfaces;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Implements
{
    public class DepartmentApiService : BaseApiService ,IDepartmentApiService
    {
        private readonly IMemoryCache cache;

        public DepartmentApiService(IMemoryCache cache, IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend") 
        {
            this.cache = cache;

        }

        public async Task<ApiResponse<List<DepartmentModel>>> GetAllDepartments()
        {
            try
            {
                if (cache.TryGetValue("Departments", out ApiResponse<List<DepartmentModel>> cachedData))
                {
                    return cachedData;
                }

                var response =await SendAuthorizedRequestAsync(() => client.GetAsync("department/all"));

                if (response == null)
                {
                    return new ApiResponse<List<DepartmentModel>>
                    {
                        Success = false,
                        Message = "Session expired"
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<DepartmentModel>>
                    {
                        Success = false,
                        Message = "Failed to fetch departments"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DepartmentModel>>>() ?? new ApiResponse<List<DepartmentModel>>
                    {
                        Success = false,
                        Message = "No response"
                    };

                if (result.Success)
                {
                    cache.Set(
                        "Departments",
                        result,
                        TimeSpan.FromHours(1));
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
