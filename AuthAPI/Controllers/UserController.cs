using AuthAPI.DTOs;
using AuthAPI.Services.Implements;
using AuthAPI.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    [Route("api/user")]
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
                var result =
                    await userService.ActivateAccount(loginDto);

                if (!result.Success)
                {
                    return result.Message switch
                    {
                        "Employee not found"
                            => NotFound(result),

                        "Account already activated"
                            => BadRequest(result),

                        _ => BadRequest(result)
                    };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = ex.Message
                    });
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await userService.RefreshToken(refreshTokenDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("signOut")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var result = await userService.SignOut(email);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }




    }
}
