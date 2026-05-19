using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployeeAsync(CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await service.AddEmployeeAsync(dto);

            if(result)
                return Created("", "Employee created successfully");

            return BadRequest("Unable to create employee");
            
            
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine(dto.Role);

            var role =  User.FindFirst(ClaimTypes.Role)?.Value;

            var loggedInEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Admin can update anyone
            if (role == "Admin")
            {
                var result =
                    await service.UpdateEmployeeAsync(id, dto);

                return result
                    ? Ok("Updated")
                    : NotFound();
            }

            // Employee can update only own profile
            if (loggedInEmail != dto.Email)
            {
                return Forbid(
                    "You can update only your own profile");
            }
            var update = await service.UpdateEmployeeAsync(id, dto);
            Console.WriteLine("---------------------------------");
            Console.WriteLine(dto.Role);
            return (update) ? Ok("Updated") : NotFound();
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await service.DeleteEmployeeAsync(id);

            return (result) ? Ok("Deleted") : NotFound();

        }

        [HttpGet]
        [Route("CheckEmailExists/{email}")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await service.CheckEmailExistsAsync(email);

            return (result) ? Ok("Email Exsist") : NotFound("Email Not Found");
        }

        [HttpGet]
        [Route("CheckEmployeeIdExists/{id}")]
        public async Task<IActionResult> CheckEmployeeIdExists(string id)
        {
            var result = await service.CheckEmployeeIdExistsAsync(id);

            return (result) ? Ok("Id Exsist") : NotFound("Id Not Found");
        }

        [HttpGet]
        [Route("CheckPhoneExists")]
        public async Task<IActionResult> CheckPhoneExists([FromQuery] string phoneNumber, [FromQuery] string? id)
        {
            var exists = await service.CheckPhoneExistsAsync(phoneNumber, id);

            if (exists)
                return Conflict("Phone number already exists");

            return Ok("Phone number is available");
        }

    }
}
