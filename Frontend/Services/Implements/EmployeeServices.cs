using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Frontend.Services.Interfaces;

namespace Frontend.Services.Implements
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeApiService employeeApi;

        public EmployeeServices(IEmployeeApiService employeeApi)
        {
            this.employeeApi = employeeApi;
        }

        public async Task<EmployeeListViewModel> GetAllEmployees(string search, int page, int pageSize)
        {
            search = search?.Trim();

            var result = await employeeApi.GetAllEmployees(search, page, pageSize);

            return new EmployeeListViewModel
            {
                Employees = result.Employees,
                PageSize = pageSize,
                CurrentPage = page,
                Search = search,
                StatusCode = result.StatusCode,
                TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize)
            };
        }

        public async Task<(EmployeeModel? Employee, int StatusCode)> GetEmployeeById(string id)
        {
            var result = await employeeApi.GetEmployeeById(id);

            return result;
        }

        public async Task<bool> AddNewEmployee(EmployeeModel model)
        {
            return await employeeApi.AddNewEmployee(model);
        }

        public async Task<int> DeleteEmployee(string employeeId)
        {
            return await employeeApi.DeleteEmployee(employeeId);
        }

        public async Task<int> UpdateEmployee(string id, UpdateEmployeeModel employeeModel)
        {
            return await employeeApi.UpdateEmployee(id, employeeModel);
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            var exists = await employeeApi.CheckEmailExists(email);

            return !exists;
        }

        public async Task<bool> IsEmployeeIdAvailable(string employeeId)
        {
            var exists = await employeeApi.CheckEmployeeIdExists(employeeId);

            return !exists;
        }

        public async Task<bool> IsPhoneAvailable(string phoneNumber, string? employeeId)
        {
            var exists = await employeeApi.CheckPhoneExists(phoneNumber, employeeId);

            return !exists;
        }



    }
}
