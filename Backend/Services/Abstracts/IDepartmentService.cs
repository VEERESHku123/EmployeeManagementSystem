using Backend.Data.Entities;
using Backend.DTOs.Common;

namespace Backend.Services.Abstracts
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<DepartmentEntity>>> GetAllManagersAsync();
    }
}