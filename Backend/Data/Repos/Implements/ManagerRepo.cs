using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly AppDbContext context;

        public ManagerRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<LeaveRequestEntity>> GetTeamLeaveRequests(string managerId)
        {
            return await context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.Employee.ManagerId == managerId && lr.Status == "Pending")
                .ToListAsync();
        }

        public async Task<LeaveRequestEntity?> GetLeaveRequestByIdAsync(int leaveRequestId)
        {
            return await context.LeaveRequests.FirstOrDefaultAsync(x => x.LeaveRequestId == leaveRequestId);
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequestEntity leaveRequest)
        {
            context.LeaveRequests.Update(leaveRequest);
            await context.SaveChangesAsync();
        }
    }
}
