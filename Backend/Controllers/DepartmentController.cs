using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        public DepartmentController(DepartmentService service)
        {
            Service = service;
        }

        public DepartmentService Service { get; set; }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await Service.GetAllManagersAsync();

            return (result != null) ? Ok(result) : NoContent();
        }
    }
}
