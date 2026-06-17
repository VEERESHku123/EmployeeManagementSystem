using Backend.Data.Entities;
using Backend.DTOs.EmployeeLeave;

namespace Backend.Data.Repos.Abstracts
{
    public interface ILeaveRepo
    {
        Task AddLeaveRequestAsync(LeaveRequestEntity leaveRequest);

        Task<List<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(string employeeId);

        Task<List<LeaveRequestEntity>> GetLeaveHistoryAsync(string employeeId,string? status = null);
    }
}
