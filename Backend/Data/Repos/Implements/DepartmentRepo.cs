using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly AppDbContext context;
        public DepartmentRepo(AppDbContext context)
        {
            this.context = context;
        }

        public AppDbContext Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<List<DepartmentEntity>> GetAllAsync()
        {
            try
            {
                return await context.Departments.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<DepartmentEntity?> GetByNameAsync(string departmentName)
        {
            return await context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
        }
    }
}
