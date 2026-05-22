using AuthAPI.DTOs;
using AuthAPI.Services.Implements;
using AuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JwtService jwtService;
        private readonly IUserService userService;

        public UserController(JwtService jwtService , IUserService userService)
        {
            this.jwtService = jwtService;
            this.userService = userService;
        }


        [HttpPost("microsoft-signin")]
        public async Task<IActionResult> MicrosoftSignIn([FromBody] MicrosoftSignInRequest request)
        {
            try
            {
                var result = await userService.MicrosoftLogin(request);

                if (!result.Success)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {

                return Problem(e.Message, "", 500);
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await userService.Login(loginDto);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                return Ok(result);
            }
            catch(Exception e)
            {
                return Problem(e.Message, "", 500);
            }
        }

        [HttpPost]
        [Route("activateAccount")]
        public async Task<IActionResult> ActivateAccount(LoginDto loginDto)
        {
            try
            {
                var result = await userService.ActivateAccount(loginDto);

                if(!result.success)
                {
                    return result.message switch
                    {
                        "Employee not found"
                            => NotFound(result.message),

                        "Account already activated"
                            => BadRequest(result.message),

                        _ => BadRequest(result.message)
                    };
                }

                return Ok(result.message);
            }
            catch (Exception e)
            {
                return Problem(e.Message, "", 500);
            }
        }



    }
}
