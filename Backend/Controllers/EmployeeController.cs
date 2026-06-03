using Backend.DTOs.Common;
using Backend.DTOs.Employee;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService; 
        public EmployeeController(IEmployeeService service)
        {
            this.employeeService = service;
        }

        

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] string? search, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await employeeService.GetAllEmployeeAsync(search, page, pageSize);

            return Ok(result);
            
        }

        [HttpGet]
        [Route("{employeeId?}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeById(string? employeeId)
        {
            if(employeeId == null) employeeId = User.FindFirst("employeeId")?.Value;

            var result = await employeeService.GetEmployeeByIdAsync(employeeId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployeeAsync(CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var result = await employeeService.AddEmployeeAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created("", result);


        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }


            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var loggedInEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Admin can update anyone
            if (role == "Admin")
            {
                var result = await employeeService.UpdateEmployeeAsync(id, dto);

                return result.Success ? Ok(result) : BadRequest(result);
            }

            // Employee can update only own profile
            if (loggedInEmail != dto.CompanyEmail)
            {
                return Forbid("You can update only your own profile");
            }

            var update = await employeeService.UpdateEmployeeAsync(id, dto);

            return update.Success ? Ok(update) : BadRequest(update);
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await employeeService.DeleteEmployeeAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);

        }

        [HttpGet]
        [Route("CheckEmailExists/{email}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await employeeService.CheckEmailExistsAsync(email);

            return (result) ? Ok("Email Exsist") : NotFound("Email Not Found");
        }

        [HttpGet]
        [Route("CheckEmployeeIdExists/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CheckEmployeeIdExists(string id)
        {
            var result = await employeeService.CheckEmployeeIdExistsAsync(id);

            return (result) ? Ok("Id Exsist") : NotFound("Id Not Found");
        }

        [HttpGet]
        [Route("CheckPhoneExists")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CheckPhoneExists([FromQuery] string phoneNumber, [FromQuery] string? id)
        {
            var exists = await employeeService.CheckPhoneExistsAsync(phoneNumber, id);

            if (exists)
                return Conflict("Phone number already exists");

            return Ok("Phone number is available");
        }

    }
}
