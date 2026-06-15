using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.DTOs.Leave;

namespace Backend.Services.Abstracts
{
    public interface ILeaveService
    {
        Task<ApiResponse<int>> ApplyLeaveAsync(ApplyLeaveDto dto);
        Task<ApiResponse<List<LeaveRequestListDto>>> GetLeaveRequestsByManagerIdAsync(string managerId);
    }
}