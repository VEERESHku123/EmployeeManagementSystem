using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class ManagerRepo : IManagerRepo
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
