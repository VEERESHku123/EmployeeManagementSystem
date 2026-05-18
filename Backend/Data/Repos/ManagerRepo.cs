using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class ManagerRepo
    {
        public EmployeeDbContext Context { get; set; }

        public ManagerRepo(EmployeeDbContext context)
        {
            Context = context;
        }

        

        public async Task<List<ManagerEntity>> GetAllAsync()
        {
            try
            {
                return await Context.Managers.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}
