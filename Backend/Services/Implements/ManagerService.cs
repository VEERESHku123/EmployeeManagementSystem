using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepo managerRepo;
        public ManagerService(IManagerRepo repo)
        {
            managerRepo = repo;
        }



        public async Task<ApiResponse<List<ManagerEntity>>> GetAllManagersAsync()
        {
            var managers = await managerRepo.GetAllAsync();

            return new ApiResponse<List<ManagerEntity>>
            {
                Success = true,
                Message = "Managers fetched successfully",
                Data = managers
            };
        }
    }
}
