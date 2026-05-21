using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IEmployeeApiService
    {
        Task<bool> AddNewEmployee(EmployeeModel model);
        Task<bool> CheckEmailExists(string email);
        Task<bool> CheckEmployeeIdExists(string employeeId);
        Task<bool> CheckPhoneExists(string phoneNumber, string? employeeId);
        Task<int> DeleteEmployee(string id);
        Task<(List<EmployeeModel> Employees, int TotalCount, int StatusCode)> GetAllEmployees(string searchTerm, int page, int pageSize);
        Task<(EmployeeModel? Employee, int StatusCode)> GetEmployeeById(string id);
        Task<int> UpdateEmployee(string id, UpdateEmployeeModel model);
    }
}