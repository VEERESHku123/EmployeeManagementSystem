using Backend.Data.Models;
using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<DepartmentEntity>>> GetAllManagersAsync();
    }
}