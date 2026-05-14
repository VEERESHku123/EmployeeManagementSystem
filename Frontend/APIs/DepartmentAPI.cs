using Frontend.Models;

namespace Frontend.APIs
{
    public class DepartmentAPI
    {
        private readonly HttpClient client;

        public DepartmentAPI(IHttpClientFactory factory)
        {
            client = factory.CreateClient("BackEnd");
        }

        public async Task<(List<DepartmentModel> departmentList, int StatusCode)> SendAllDepartments()
        {
            try
            {
                var response = await client.GetAsync("department/all");

                if (!response.IsSuccessStatusCode)
                {
                    return (new List<DepartmentModel>(), (int)response.StatusCode);
                }

                var departments = await response.Content.ReadFromJsonAsync<List<DepartmentModel>>();

                return (departments ?? new List<DepartmentModel>(), (int)response.StatusCode);
            }
            catch
            {
                throw;
            }
        }
    }
}
