using Backend.DTOs.Common;
using Backend.DTOs.Manager;

namespace Backend.Services.Abstracts
{
    public interface IManagerService
    {
        Task<ApiResponse<List<LeaveRequestDto>>> GetTeamLeaveRequests(string managerId);
        Task<ApiResponse<string>> ApproveOrRejectLeaveAsync(string managerEmployeeId, LeaveApprovalRequest request);
    }
}