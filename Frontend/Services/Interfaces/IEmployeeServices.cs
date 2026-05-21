using Frontend.Models;

namespace Frontend.Services.Interfaces
{
    public interface IEmployeeServices
    {
        Task<bool> AddNewEmployee(EmployeeModel model);
        Task<int> DeleteEmployee(string employeeId);
        Task<EmployeeListViewModel> GetAllEmployees(string search, int page, int pageSize);
        Task<(EmployeeModel? Employee, int StatusCode)> GetEmployeeById(string id);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsEmployeeIdAvailable(string employeeId);
        Task<bool> IsPhoneAvailable(string phoneNumber, string? employeeId);
        Task<int> UpdateEmployee(string id, UpdateEmployeeModel employeeModel);
    }
}