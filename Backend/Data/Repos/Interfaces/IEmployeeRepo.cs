using Backend.Data.Context;
using Backend.Data.Models;

namespace Backend.Data.Repos.Interfaces
{
    public interface IEmployeeRepo
    {

        Task<bool> AddAsync(EmployeeEntity entity);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckEmployeeIdExistsAsync(string id);
        Task<bool> CheckPhoneExistsAsync(string phoneNumber, string id);
        Task<bool> DeleteByIdAsync(string id);
        Task<(List<EmployeeEntity> Data, int TotalCount)> GetAllAsync(string searchTerm, int page, int pageSize);
        Task<EmployeeEntity> GetById(string id);
        Task<bool> UpdateAsync(string id, EmployeeEntity entity);
    }
}