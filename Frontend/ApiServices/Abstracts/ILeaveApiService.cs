using Frontend.Models.Common;
using Frontend.Models.Leave;

namespace Frontend.ApiServices.Abstracts
{
    public interface ILeaveApiService
    {
        Task<ApiResponse<List<LeaveHistoryModel>>> GetLeaveHistory(string status = "Pending");
        Task<ApiResponse<List<LeaveBalanceModel>>> GetEmployeeLeaveBalancesAsync();
        Task<ApiResponse<int>> ApplyLeaveAsync(ApplyLeaveModel model);
    }
}