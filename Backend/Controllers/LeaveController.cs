using Backend.DTOs.Leave;
using Backend.Services.Abstracts;
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
        public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveDto dto)
        {
            var response = await leaveService.ApplyLeaveAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("manager/{managerId}")]
        public async Task<IActionResult> GetLeaveRequestsByManager(string managerId)
        {
            var response = await leaveService.GetLeaveRequestsByManagerIdAsync(managerId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
