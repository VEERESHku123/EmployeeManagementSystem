using Backend.Data.Context;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos
{
    public class ManagerRepo
    {
        public AppDbContext Context { get; set; }

        public ManagerRepo(AppDbContext context)
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
