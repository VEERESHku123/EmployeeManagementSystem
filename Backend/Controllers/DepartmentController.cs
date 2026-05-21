using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService departmentService;
        public DepartmentController(DepartmentService service)
        {
            departmentService = service;
        }

        

        [HttpGet("all")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await departmentService.GetAllManagersAsync();

            return (result != null) ? Ok(result) : NoContent();
        }
    }
}
