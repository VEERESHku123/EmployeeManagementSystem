using Backend.Data.Entities;

namespace Backend.Data.Repos.Abstracts
{
    public interface IManagerRepo
    {
        Task<List<LeaveRequestEntity>> GetTeamLeaveRequests(string managerId);
        Task<LeaveRequestEntity?> GetLeaveRequestByIdAsync(int leaveRequestId);
        Task UpdateLeaveRequestAsync(LeaveRequestEntity leaveRequest);
    }
}