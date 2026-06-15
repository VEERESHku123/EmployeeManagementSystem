using Frontend.Models.Common;
using Frontend.Models.Employee;

namespace Frontend.ApiServices.Abstracts
{
    public interface IEmployeeApiService
    {
        Task<ApiResponse<EmployeeModel>> AddNewEmployee(EmployeeModel model);
        Task<bool> CheckEmailExists(string email);
        Task<bool> CheckEmployeeIdExists(string employeeId);
        Task<bool> CheckPhoneExists(string phoneNumber, string? employeeId);
        Task<ApiResponse<bool>> DeleteEmployee(string id);
        Task<ApiResponse<EmployeePaginationData>> GetAllEmployees(string searchTerm, int page, int pageSize);
        Task<ApiResponse<EmployeeModel>> GetEmployeeById(string? employeeId);
        Task<ApiResponse<UpdateEmployeeModel>> UpdateEmployee(string id, UpdateEmployeeModel model);

        Task<ApiResponse<List<DesignationModel>>> GetAllDesignations();
        Task<ApiResponse<EmployeeUploadResultModel>> UploadEmployeesAsync(IFormFile file);
        Task<byte[]> DownloadTemplateAsync();

        Task<byte[]> DownloadInvalidFileAsync(string fileName);

        Task<ApiResponse<List<ManagerModel>>> SendAllManagers();
    }
}