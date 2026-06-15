using Backend.Data.Entities;
using Backend.DTOs.Employee;

namespace Backend.Data.Repos.Abstracts
{
    public interface ILeaveRepo
    {
        Task AddLeaveRequestAsync(LeaveRequestEntity leaveRequest);
        Task<List<LeaveRequestEntity>> GetLeaveRequestsByManagerIdAsync(string managerId);
    }
}