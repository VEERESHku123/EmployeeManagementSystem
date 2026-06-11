using AuthAPI.DTOs.ForgetPassword;
using AuthAPI.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/password-reset")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService passwordResetService;

        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            this.passwordResetService = passwordResetService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await passwordResetService.ForgotPasswordAsync(request.Email);

            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {
            var response = await passwordResetService.VerifyOtpAsync(request.Email, request.Otp);

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var response = await passwordResetService.ResetPasswordAsync(request.ResetToken, request.NewPassword);

            return Ok(response);
        }
    }
}
