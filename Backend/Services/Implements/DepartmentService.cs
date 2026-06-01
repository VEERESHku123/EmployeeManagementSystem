using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly ILogger<DepartmentService> logger;
        public DepartmentService(IDepartmentRepo repo, ILogger<DepartmentService> logger)
        {
            departmentRepo = repo;
            this.logger = logger;
        }



        public async Task<ApiResponse<List<DepartmentEntity>>> GetAllManagersAsync()
        {
            try
            {
                logger.LogInformation("Fetching all departments");

                var departments = await departmentRepo.GetAllAsync();

                logger.LogInformation(
                    "Successfully fetched {DepartmentCount} departments",
                    departments.Count);

                return new ApiResponse<List<DepartmentEntity>>
                {
                    Success = true,
                    Message = "Departments fetched successfully",
                    Data = departments
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while fetching departments");

                throw;
            }
        }
    }

}
