using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        public ManagerController(ManagerService service)
        {
            Service = service;
        }

        public ManagerService Service { get; set; }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await Service.GetAllManagersAsync();

            return (result != null) ? Ok(result) : NoContent();
        }
    }
}
