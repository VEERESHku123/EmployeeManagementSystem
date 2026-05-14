using Backend.Data.Models;
using Backend.Data.Repos;

namespace Backend.Services
{
    public class ManagerService
    {
        public ManagerService(ManagerRepo repo)
        {
            Repo = repo;
        }

        public ManagerRepo Repo { get; set; }

        public async Task<List<ManagerEntity>> GetAllManagersAsync()
        {
            try
            {
                return await Repo.GetAllAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
