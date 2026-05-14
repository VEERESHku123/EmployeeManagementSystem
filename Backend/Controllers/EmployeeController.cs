using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController(IEmployeeService service)
        {
            this.service = service;
        }

        public IEmployeeService service { get; set; }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] string? search, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await service.GetAllEmployeeAsync(search, page, pageSize);

            var response = new PagedEmployeeResponse
            {
                Employees = result.Data,
                TotalCount = result.TotalCount
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var result = await service.GetEmployeeByIdAsync(id);
            return (result != null) ? Ok(result) : NotFound($"Employee ID: {id} Not Found");
            
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> InsertPostAsync(CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.AddEmployeeAsync(dto);

            return (result) ? Created() : throw new Exception("Something went Wrong");
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] CreateEmployeeDTO dto)
        {

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.UpdateEmployeeAsync(id, dto);
            Console.WriteLine(result);
            return (result) ? Ok("Updated") : NotFound();
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await service.DeleteEmployeeAsync(id);

            return (result) ? Ok("Deleted") : NotFound();

        }

        [HttpGet]
        [Route("search/{searchTerm}")]
        public async Task<IActionResult> searchEmployee([FromQuery]string searchTerm)
        {
            var result = await service.SearchAsync(searchTerm);

            return Ok(result);
        }
    }
}
