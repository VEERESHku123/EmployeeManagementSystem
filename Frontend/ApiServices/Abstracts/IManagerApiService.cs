using Frontend.Models.Common;
using Frontend.Models.Leave;
using Frontend.Models.Manager;

namespace Frontend.ApiServices.Abstracts
{
    public interface IManagerApiService
    {
        Task<ApiResponse<List<LeaveRequestModel>>> GetTeamLeaveRequests();
        Task<ApiResponse<string>> ApproveOrRejectLeaveAsync(LeaveApprovalRequestModel leaveApprovalRequest);
    }
}