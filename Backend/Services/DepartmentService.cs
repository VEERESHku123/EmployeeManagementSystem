using Backend.Data.Models;
using Backend.Data.Repos;

namespace Backend.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepo departmentRepo;
        public DepartmentService(DepartmentRepo repo)
        {
            departmentRepo = repo;
        }

        

        public async Task<List<DepartmentEntity>> GetAllManagersAsync()
        {
            try
            {
                return await departmentRepo.GetAllAsync();
            }
            catch
            {
                throw;
            }
        }
    }

}
