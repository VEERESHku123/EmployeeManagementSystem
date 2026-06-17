using Backend.Services.Implements;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        public DepartmentController(IDepartmentService service)
        {
            departmentService = service;
        }

        

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await departmentService.GetAllManagersAsync();

            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}
