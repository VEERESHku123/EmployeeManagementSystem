using AutoMapper;
using Backend.Data.Repos;
using Backend.DTOs;

namespace Backend.Services
{
    public interface IEmployeeService
    {

        Task<bool> AddEmployeeAsync(CreateEmployeeDTO employeeDTO);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckEmployeeIdExistsAsync(string id);
        Task<bool> CheckPhoneExistsAsync(string phoneNumber, string? id);
        Task<bool> DeleteEmployeeAsync(string id);
        Task<(List<EmployeeDTO> Data, int TotalCount)> GetAllEmployeeAsync(string searchTerm, int page, int pageSize);
        Task<EmployeeDTO> GetEmployeeByIdAsync(string id);
        Task<bool> UpdateEmployeeAsync(string id, CreateEmployeeDTO dto);
    }
}