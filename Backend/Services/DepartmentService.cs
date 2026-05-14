using Backend.Data.Models;
using Backend.Data.Repos;

namespace Backend.Services
{
    public class DepartmentService
    {
        public DepartmentService(DepartmentRepo repo)
        {
            Repo = repo;
        }

        public DepartmentRepo Repo { get; set; }

        public async Task<List<DepartmentEntity>> GetAllManagersAsync()
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
