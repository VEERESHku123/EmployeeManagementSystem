using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using System.Net;
using System.Net.Http.Headers;
namespace Frontend.ApiServices.Implements
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor context;

        public EmployeeApiService(IHttpClientFactory factory, IHttpContextAccessor context)
        {
            client = factory.CreateClient("BackEnd");
            this.context = context;
        }

        public async Task<(List<EmployeeModel> Employees, int TotalCount, int StatusCode)> GetAllEmployees(
            string searchTerm, int page, int pageSize)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            var url = $"employee/all?search={Uri.EscapeDataString(searchTerm ?? "")}&page={page}&pageSize={pageSize}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (new List<EmployeeModel>(), 0, (int)response.StatusCode);
            }

            var result = await response.Content.ReadFromJsonAsync<EmployeePagedResponseModel>();
            return (
                result?.Employees ?? new(),
                result?.TotalCount ?? 0,
                (int)response.StatusCode
            );
        }

        public async Task<(EmployeeModel? Employee, int StatusCode)> GetEmployeeById(string id)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("employee/add", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<int> UpdateEmployee(string id, UpdateEmployeeModel model)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);

            var response = await client.PutAsJsonAsync($"employee/update/{id}", model);

            return (int)response.StatusCode;
        }

        public async Task<int> DeleteEmployee(string id)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);

            var response = await client.DeleteAsync($"employee/delete/{id}");

            return (int)response.StatusCode;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);
            var response = await client.GetAsync($"employee/CheckEmailExists/{email}");

            return response.StatusCode == HttpStatusCode.OK;

        }

        public async Task<bool> CheckEmployeeIdExists(string employeeId)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);
            var response = await client.GetAsync($"employee/CheckEmployeeIdExists/{employeeId}");

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CheckPhoneExists(string phoneNumber, string? employeeId)
        {
            var token = context.HttpContext?.Session.GetString("JwtToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);

            var url = $"employee/CheckPhoneExists?phoneNumber={phoneNumber}&id={employeeId}";

            var response = await client.GetAsync(url);

            return response.StatusCode == HttpStatusCode.Conflict;
        }
    }
}
