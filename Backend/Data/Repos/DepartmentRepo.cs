using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class DepartmentRepo
    {
        public EmployeeDbContext Context { get; set; }
        public DepartmentRepo(EmployeeDbContext context)
        {
            Context = context;
        }


        public async Task<List<DepartmentEntity>> GetAllAsync()
        {
            try
            {
                return await Context.Departments.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}
