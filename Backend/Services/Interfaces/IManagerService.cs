using Backend.Data.Models;
using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ApiResponse<List<ManagerEntity>>> GetAllManagersAsync();
    }
}