using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Net.Http.Headers;

namespace Frontend.ApiServices.Implements
{
    public class DepartmentApiService : IDepartmentApiService
    {
        private readonly HttpClient client;
        private readonly IMemoryCache cache;
        private readonly IHttpContextAccessor context;

        public DepartmentApiService(IMemoryCache cache, IHttpClientFactory factory, IHttpContextAccessor context)
        {
            client = factory.CreateClient("BackEnd");
            this.cache = cache;
            this.context = context;

        }

        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            try
            {
                

                if (!cache.TryGetValue("Departments", out List<DepartmentModel> departmentList))
                {
                    var token = context.HttpContext?.Session.GetString("AccessToken");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    departmentList = await client.GetFromJsonAsync<List<DepartmentModel>>("department/all");

                    cache.Set(
                        "Departments",
                        departmentList,
                        TimeSpan.FromHours(1));
                }

                return departmentList ?? new List<DepartmentModel>();
            }
            catch
            {
                throw;
            }
        }
    }
}
