using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.DTOs.EmployeeLeave;

namespace Backend.Services.Abstracts
{
    public interface ILeaveService
    {
        Task<ApiResponse<int>> ApplyLeaveAsync(ApplyLeaveDto dto);

        Task<ApiResponse<List<LeaveBalanceDto>>> GetEmployeeLeaveBalancesAsync(string employeeId);

        Task<ApiResponse<List<LeaveHistoryDto>>> GetLeaveHistoryAsync(string employeeId, string? status = null);
    }
}