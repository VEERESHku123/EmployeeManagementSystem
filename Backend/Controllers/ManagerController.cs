using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService managerService;
        public ManagerController(ManagerService service)
        {
            managerService = service;
        }

        


        [HttpGet("all")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await managerService.GetAllManagersAsync();

            return (result != null) ? Ok(result) : NoContent();
        }
    }
}
