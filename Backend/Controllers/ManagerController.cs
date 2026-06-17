using Backend.DTOs.Manager;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService managerService;

        public ManagerController(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        [HttpGet("leaveRequests")]
        [Authorize]
        public async Task<IActionResult> GetLeaveRequestsByManager()
        {
            var managerId = User.FindFirst("employeeId")?.Value;

            var response = await managerService.GetTeamLeaveRequests(managerId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPut("leave/approve-reject")]
        public async Task<IActionResult> ApproveOrRejectLeave([FromBody] LeaveApprovalRequest request)
        {
            var managerEmployeeId = User.FindFirst("employeeId")?.Value;

            var response = await managerService.ApproveOrRejectLeaveAsync(managerEmployeeId, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
