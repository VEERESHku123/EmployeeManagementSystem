using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly AppDbContext context;

        public ManagerRepo(AppDbContext context)
        {
            this.context = context;
        }



        public async Task<List<ManagerEntity>> GetAllAsync()
        {
            try
            {
                return await context.Managers.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ManagerEntity?> GetByNameAsync(string managerName)
        {
            return await context.Managers.FirstOrDefaultAsync(m => m.ManagerName == managerName);
        }
    }
}
