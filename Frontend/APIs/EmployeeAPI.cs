using Frontend.Models;

namespace Frontend.APIs
{
    public class EmployeeAPI
    {
        private readonly HttpClient client;

        public EmployeeAPI(IHttpClientFactory factory)
        {
            client = factory.CreateClient("BackEnd");
        }

        public async Task<(List<EmployeeModel> Employees, int TotalCount, int StatusCode)> SendAllEmployee(
    string searchTerm, int page, int pageSize)
        {
            var url = $"employee/all?search={Uri.EscapeDataString(searchTerm ?? "")}&page={page}&pageSize={pageSize}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (new List<EmployeeModel>(), 0, (int)response.StatusCode);
            }

            var result = await response.Content.ReadFromJsonAsync<EmployeePagedResponseModel>();

            return (
                result?.Employees ?? new List<EmployeeModel>(),
                result?.TotalCount ?? 0,
                (int)response.StatusCode
            );
        }

        public async Task<(EmployeeModel? Employee, int StatusCode)> SendEmployeeById(string id)
        {
            var response = await client.GetAsync($"employee/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return (null, (int)response.StatusCode);
            }

            var employee = await response.Content.ReadFromJsonAsync<EmployeeModel>();

            return (employee, (int)response.StatusCode);
        }

        public async Task<bool> AddNewEmployee(EmployeeModel model)
        {
            var response = await client.PostAsJsonAsync("employee/add", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<int> UpdateEmployee(string id, EmployeeModel model)
        {
            var response = await client.PutAsJsonAsync($"employee/update/{id}", model);
            Console.WriteLine(response.IsSuccessStatusCode);
            return (int)response.StatusCode;
        }

        public async Task<int> DeleteEmployee(string id)
        {
            var response = await client.DeleteAsync($"employee/delete/{id}");

            return (int)response.StatusCode;
        }
    }
}
