using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IEmployeeApiService
    {
        Task<ApiResponse<EmployeeModel>> AddNewEmployee(EmployeeModel model);
        Task<bool> CheckEmailExists(string email);
        Task<bool> CheckEmployeeIdExists(string employeeId);
        Task<bool> CheckPhoneExists(string phoneNumber, string? employeeId);
        Task<ApiResponse<bool>> DeleteEmployee(string id);
        Task<ApiResponse<EmployeePaginationData>> GetAllEmployees(string searchTerm, int page, int pageSize);
        Task<ApiResponse<EmployeeModel>> GetEmployeeById(string id);
        Task<ApiResponse<UpdateEmployeeModel>> UpdateEmployee(string id, UpdateEmployeeModel model);
    }
}