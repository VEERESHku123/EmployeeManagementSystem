using Backend.Data.Models;
using Backend.Data.Repos;

namespace Backend.Services
{
    public class ManagerService
    {
        private readonly ManagerRepo managerRepo;
        public ManagerService(ManagerRepo repo)
        {
            managerRepo = repo;
        }

        

        public async Task<List<ManagerEntity>> GetAllManagersAsync()
        {
            try
            {
                return await managerRepo.GetAllAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
