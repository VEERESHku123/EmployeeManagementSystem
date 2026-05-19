using AuthAPI.DTOs;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService jwtService;
        private readonly EmployeeService employeeService;

        public AuthController(JwtService jwtService, EmployeeService employeeService)
        {
            this.jwtService = jwtService;
            this.employeeService = employeeService;
        }


        [HttpPost("microsoft-signin")]
        public async Task<IActionResult> MicrosoftSignIn([FromBody] MicrosoftSignInRequest request)
        {
            var role = await employeeService.CheckEmailExistsAsync(request.Email);
            if (role == null) {
                return Unauthorized();
            }

            var response = jwtService.GenerateToken(
                request.Email,
                request.Name,
                role);

            return Ok(response);
        }
    }
}
