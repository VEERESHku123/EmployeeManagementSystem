using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class ManagerRepo
    {
        public ManagerRepo(EmployeeDbContext context)
        {
            Context = context;
        }

        public EmployeeDbContext Context { get; set; }

        public async Task<List<ManagerEntity>> GetAllAsync()
        {
            return await Context.Managers.ToListAsync();
        }

    }
}
