using Backend.Data.Entities;
using Backend.DTOs.Common;

namespace Backend.Services.Abstracts
{
    public interface IManagerService
    {
        Task<ApiResponse<List<ManagerEntity>>> GetAllManagersAsync();
    }
}