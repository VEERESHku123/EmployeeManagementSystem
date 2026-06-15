using AutoMapper;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.DTOs.Leave;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepo leaveRepo;
        private readonly ILogger<LeaveService> logger;
        private readonly IMapper mapper;

        public LeaveService(ILeaveRepo leaveRepo, ILogger<LeaveService> logger, IMapper mapper)
        {
            this.leaveRepo = leaveRepo;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<int>> ApplyLeaveAsync(ApplyLeaveDto dto)
        {
            try
            {
                logger.LogInformation("Leave application received for EmployeeId: {EmployeeId}", dto.EmployeeId);

                if (dto.StartDate > dto.EndDate)
                {
                    logger.LogWarning("Invalid leave dates for EmployeeId: {EmployeeId}", dto.EmployeeId);

                    return new ApiResponse<int>
                    {
                        Success = false,
                        Message = "Start date cannot be greater than end date."
                    };

                }

                int totalDays = (dto.EndDate - dto.StartDate).Days + 1;

                LeaveRequestEntity leaveRequest = new LeaveRequestEntity
                {
                    EmployeeId = dto.EmployeeId,
                    LeaveTypeId = dto.LeaveTypeId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    TotalDays = totalDays,
                    Reason = dto.Reason,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                await leaveRepo.AddLeaveRequestAsync(leaveRequest);

                logger.LogInformation("Leave applied successfully. LeaveRequestId: {LeaveRequestId}", leaveRequest.LeaveRequestId);

                return new ApiResponse<int>
                {
                    Success = true,
                    Message = "Leave applied successfully.",
                    Data = leaveRequest.LeaveRequestId
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while applying leave for EmployeeId: {EmployeeId}",
                    dto.EmployeeId);

                return new ApiResponse<int>
                {
                    Success = false,
                    Message = "Failed to apply leave.",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<LeaveRequestListDto>>> GetLeaveRequestsByManagerIdAsync(string managerId)
        {
            try
            {
                var leaveRequests = await leaveRepo.GetLeaveRequestsByManagerIdAsync(managerId);

                var leaveRequestDtos = mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                return new ApiResponse<List<LeaveRequestListDto>>
                {
                    Success = true,
                    Message = leaveRequestDtos.Any()
                        ? "Leave requests fetched successfully."
                        : "No leave requests found.",
                    Data = leaveRequestDtos
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error fetching leave requests for manager {ManagerId}",
                    managerId);

                return new ApiResponse<List<LeaveRequestListDto>>
                {
                    Success = false,
                    Message = "Failed to fetch leave requests.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
