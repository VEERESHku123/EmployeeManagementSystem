using Backend.Data.Entities;
using Backend.DTOs.Common;
using Backend.DTOs.Employee;

namespace Backend.Services.Abstracts
{
    public interface IEmployeeService
    {

        Task<ApiResponse<CreateEmployeeDTO>> AddEmployeeAsync(CreateEmployeeDTO employeeDTO);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckEmployeeIdExistsAsync(string id);
        Task<bool> CheckPhoneExistsAsync(string phoneNumber, string? id);
        Task<ApiResponse<bool>> DeleteEmployeeAsync(string id);
        Task<ApiResponse<object>> GetAllEmployeeAsync(string searchTerm, int page, int pageSize);
        Task<ApiResponse<EmployeeDTO>> GetEmployeeByIdAsync(string id);
        Task<ApiResponse<CreateEmployeeDTO>> UpdateEmployeeAsync(string id, CreateEmployeeDTO dto);

        Task<ApiResponse<EmployeeUploadResultDTO>> UploadEmployeesAsync(IFormFile file);

        Task<byte[]> DownloadTemplateAsync();

        Task<ApiResponse<List<DesignationEntity>>> GetAllDesignations();
    }
}