using Backend.Services.Implements;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService managerService;
        public ManagerController(IManagerService service)
        {
            managerService = service;
        }

        


        [HttpGet("all")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await managerService.GetAllManagersAsync();

            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}
