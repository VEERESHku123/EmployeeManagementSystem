using Backend.DTOs.EmployeeLeave;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/leave")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            this.leaveService = leaveService;
        }

        [HttpPost("apply")]
        [Authorize]
        public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveDto dto)
        {
            var employeeId = User.FindFirst("employeeId")?.Value;
            dto.EmployeeId = employeeId;
            var response = await leaveService.ApplyLeaveAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        

        [HttpGet("balances")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeLeaveBalances()
        {
            var employeeId = User.FindFirst("employeeId")?.Value;

            var response = await leaveService.GetEmployeeLeaveBalancesAsync(employeeId);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("history/{status}")]
        [Authorize]
        public async Task<IActionResult> GetLeaveHistory(string status)
        {
            var employeeId = User.FindFirst("employeeId")?.Value;

            var response = await leaveService.GetLeaveHistoryAsync(employeeId, status);

            return Ok(response);
        }

    }
}
