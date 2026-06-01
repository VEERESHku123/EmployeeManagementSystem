using Backend.Data.Context;
using Backend.Data.Entities;

namespace Backend.Data.Repos.Abstracts
{
    public interface IDepartmentRepo
    {
        AppDbContext Context { get; set; }

        Task<List<DepartmentEntity>> GetAllAsync();
    }
}