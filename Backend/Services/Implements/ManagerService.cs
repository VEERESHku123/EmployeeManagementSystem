using Backend.Data.Models;
using Backend.Data.Repos.Interfaces;
using Backend.DTOs;
using Backend.Services.Interfaces;

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
            try
            {
                var managers = await managerRepo.GetAllAsync();

                return new ApiResponse<List<ManagerEntity>>
                {
                    Success = managers != null,
                    Message = "Managers fetched successfully",
                    Data = managers
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
