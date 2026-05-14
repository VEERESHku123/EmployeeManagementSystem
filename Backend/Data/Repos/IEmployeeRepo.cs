using Backend.Data.Context;
using Backend.Data.Models;

namespace Backend.Data.Repos
{
    public interface IEmployeeRepo
    {

        Task<bool> AddAsync(EmployeeEntity entity);
        Task<bool> DeleteByIdAsync(string id);
        Task<(List<EmployeeEntity> Data, int TotalCount)> GetAllAsync(string searchTerm, int page, int pageSize);
        Task<EmployeeEntity> GetById(string id);
        Task<bool> UpdateAsync(string id, EmployeeEntity entity);
        Task<List<EmployeeEntity>> SearchAsync(string searchTerm);
    }
}