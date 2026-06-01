using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class DepartmentRepo : IDepartmentRepo
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
