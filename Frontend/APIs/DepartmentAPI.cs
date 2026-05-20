using Frontend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Frontend.APIs
{
    public class DepartmentAPI
    {
        private readonly HttpClient client;
        private readonly IMemoryCache cache;

        public DepartmentAPI(IMemoryCache cache, IHttpClientFactory factory)
        {
            client = factory.CreateClient("BackEnd");
            this.cache = cache;

        }

        public async Task<List<DepartmentModel>> SendAllDepartments()
        {
            try
            {
                if(!cache.TryGetValue("Departments", out List<DepartmentModel> departmentList))
                {
                    departmentList = await client.GetFromJsonAsync<List<DepartmentModel>>("department/all");

                    cache.Set(
                        "Departments",
                        departmentList,
                        TimeSpan.FromHours(1));
                }

                return departmentList?? new List<DepartmentModel>();
            }
            catch
            {
                throw;
            }
        }
    }
}
