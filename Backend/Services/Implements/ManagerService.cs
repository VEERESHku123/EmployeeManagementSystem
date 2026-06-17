using AutoMapper;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.Manager;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepo managerRepo;
        private readonly ILogger<IManagerService> logger;
        private readonly IMapper mapper;

        public ManagerService(IManagerRepo managerRepo, ILogger<IManagerService> logger, IMapper mapper)
        {
            this.managerRepo = managerRepo;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<LeaveRequestDto>>> GetTeamLeaveRequests(string managerId)
        {
            try
            {
                logger.LogInformation("Fetching leave requests for manager {ManagerId}", managerId);

                var leaveRequests = await managerRepo.GetTeamLeaveRequests(managerId);

                var leaveRequestDtos = mapper.Map<List<LeaveRequestDto>>(leaveRequests);

                logger.LogInformation("Successfully fetched {Count} leave requests for manager {ManagerId}", leaveRequestDtos.Count,
                    managerId);

                return new ApiResponse<List<LeaveRequestDto>>
                {
                    Success = true,
                    Message = "Leave requests fetched successfully.",
                    Data = leaveRequestDtos
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while fetching leave requests for manager {ManagerId}",
                    managerId);

                return new ApiResponse<List<LeaveRequestDto>>
                {
                    Success = false,
                    Message = "An error occurred while fetching leave requests.",
                    Data = new List<LeaveRequestDto>()
                };
            }
        }


        public async Task<ApiResponse<string>> ApproveOrRejectLeaveAsync(string managerEmployeeId, LeaveApprovalRequest request)
        {
            try
            {
                logger.LogInformation(
                    "Processing leave request {LeaveRequestId} with status {Status} by manager {ManagerId}",
                    request.LeaveRequestId,
                    request.Status,
                    managerEmployeeId);

                var leaveRequest = await managerRepo.GetLeaveRequestByIdAsync(request.LeaveRequestId);

                if (leaveRequest == null)
                {
                    logger.LogWarning(
                        "Leave request not found. LeaveRequestId: {LeaveRequestId}",
                        request.LeaveRequestId);

                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Leave request not found."
                    };
                }

                if (leaveRequest.Status != "Pending")
                {
                    logger.LogWarning(
                        "Leave request {LeaveRequestId} already processed with status {Status}",
                        leaveRequest.LeaveRequestId,
                        leaveRequest.Status);

                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = $"Leave request already {leaveRequest.Status}."
                    };
                }

                if (request.Status == "Rejected" && string.IsNullOrWhiteSpace(request.ManagerRemark))
                {
                    logger.LogWarning(
                        "Manager remark missing for rejected leave request {LeaveRequestId}",
                        request.LeaveRequestId);

                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Remark is required when rejecting a leave request."
                    };
                }

                leaveRequest.Status = request.Status;
                leaveRequest.ManagerRemark = request.ManagerRemark;
                leaveRequest.ApprovedByEmployeeId = managerEmployeeId;
                leaveRequest.ApprovedDate = DateTime.UtcNow;

                await managerRepo.UpdateLeaveRequestAsync(leaveRequest);

                logger.LogInformation(
                    "Leave request {LeaveRequestId} successfully {Status} by manager {ManagerId}",
                    leaveRequest.LeaveRequestId,
                    request.Status,
                    managerEmployeeId);

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = $"Leave request {request.Status.ToLower()} successfully.",
                    Data = leaveRequest.LeaveRequestId.ToString()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing leave request {LeaveRequestId}",
                    request.LeaveRequestId);

                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the leave request.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
