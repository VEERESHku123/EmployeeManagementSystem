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

        public AuthController(JwtService jwtService)
        {
            this.jwtService = jwtService;
        }


        [HttpPost("microsoft-signin")]
        public IActionResult MicrosoftSignIn([FromBody] MicrosoftSignInRequest request)
        {
            // Check user exists in DB
            // Create user if not exists
            Console.WriteLine("0----------------1");
            var response = jwtService.GenerateToken(
                request.Email,
                request.Name);

            return Ok(response);
        }
    }
}
