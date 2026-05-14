using AutoMapper;
using Backend.Data.Repos;
using Backend.DTOs;

namespace Backend.Services
{
    public interface IEmployeeService
    {

        Task<bool> AddEmployeeAsync(CreateEmployeeDTO employeeDTO);
        Task<bool> DeleteEmployeeAsync(string id);
        Task<(List<EmployeeDTO> Data, int TotalCount)> GetAllEmployeeAsync(string searchTerm, int page, int pageSize);
        Task<EmployeeDTO> GetEmployeeByIdAsync(string id);
        Task<bool> UpdateEmployeeAsync(string id, CreateEmployeeDTO dto);

        Task<List<EmployeeDTO>> SearchAsync(string searchTerm);
    }
}