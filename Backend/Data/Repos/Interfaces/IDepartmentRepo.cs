using Backend.Data.Context;
using Backend.Data.Models;

namespace Backend.Data.Repos.Interfaces
{
    public interface IDepartmentRepo
    {
        AppDbContext Context { get; set; }

        Task<List<DepartmentEntity>> GetAllAsync();
    }
}