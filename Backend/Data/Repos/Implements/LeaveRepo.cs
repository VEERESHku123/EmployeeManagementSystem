using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.EmployeeLeave;
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

        public async Task<List<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(string employeeId)
        {
            return await (from lb in context.LeaveBalances
                          join lt in context.LeaveTypes
                              on lb.LeaveTypeId equals lt.LeaveTypeId
                          where lb.EmployeeId == employeeId
                                && lt.IsActive == true
                          select new LeaveBalanceDto
                          {
                              LeaveTypeId = lt.LeaveTypeId,
                              LeaveTypeName = lt.LeaveTypeName,
                              TotalLeaves = lb.TotalLeaves,
                              UsedLeaves = lb.UsedLeaves,
                              AvailableLeaves = lb.TotalLeaves - lb.UsedLeaves
                          })
                      .ToListAsync();
        }


        public async Task<List<LeaveRequestEntity>> GetLeaveHistoryAsync(string employeeId, string? status = null)
        {
            var query = context.LeaveRequests
                .Include(x => x.LeaveType)
                .Include(x => x.ApprovedByEmployee)
                .Where(x => x.EmployeeId == employeeId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
