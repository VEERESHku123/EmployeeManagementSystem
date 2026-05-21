using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class DepartmentRepo
    {
        public AppDbContext Context { get; set; }
        public DepartmentRepo(AppDbContext context)
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
