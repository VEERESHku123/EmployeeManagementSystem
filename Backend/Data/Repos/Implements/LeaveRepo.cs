using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Employee;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class LeaveRepo : ILeaveRepo
    {
        private readonly AppDbContext context;

        public LeaveRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddLeaveRequestAsync(LeaveRequestEntity leaveRequest)
        {
            await context.LeaveRequests.AddAsync(leaveRequest);
            await context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequestEntity>> GetLeaveRequestsByManagerIdAsync(string managerId)
        {
            return await context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.Employee.ManagerId == managerId)
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync();
        }
    }
}
