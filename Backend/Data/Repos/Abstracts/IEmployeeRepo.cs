using Backend.Data.Entities;
using Backend.DTOs.Employee;

namespace Backend.Data.Repos.Abstracts
{
    public interface IEmployeeRepo
    {

        Task<bool> AddAsync(EmployeeEntity entity);
        Task<bool> CheckCompanyEmailExistsAsync(string companyEmail);
        Task<bool> CheckPersonalEmailExistsAsync(string personalEmail);
        Task<bool> CheckEmployeeIdExistsAsync(string id);
        Task<bool> CheckPhoneExistsAsync(string phoneNumber, string? id);
        Task<bool> DeleteByIdAsync(string id);
        Task<(List<EmployeeEntity> Data, int TotalCount)> GetPagedEmployeesAsync(string searchTerm, int page, int pageSize);
        Task<EmployeeEntity> GetById(string id);
        Task<bool> UpdateAsync(string id, EmployeeEntity entity);

        Task BulkInsertEmployeesAsync(List<EmployeeEntity> employees);

        Task<List<DesignationEntity>> GetAllDesignations();
        Task<DesignationEntity?> GetByDesignationNameAsync(string designationName);


        //Manager Section
        Task<List<ManagerDto>> GetManagersAsync();
        Task<ManagerDto?> GetManagerByNameAsync(string managerName);
    }
}