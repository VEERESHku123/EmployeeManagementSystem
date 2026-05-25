using Backend.Data.Models;
using Backend.Data.Repos.Interfaces;
using Backend.DTOs;
using Backend.Services.Interfaces;

namespace Backend.Services.Implements
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo departmentRepo;
        public DepartmentService(IDepartmentRepo repo)
        {
            departmentRepo = repo;
        }



        public async Task<ApiResponse<List<DepartmentEntity>>> GetAllManagersAsync()
        {
            try
            {
                var departments = await departmentRepo.GetAllAsync();

                return new ApiResponse<List<DepartmentEntity>>
                {
                    Success = departments != null,
                    Message = "Departments fetched successfully",
                    Data = departments
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
