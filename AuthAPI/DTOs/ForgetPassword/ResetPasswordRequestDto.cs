namespace AuthAPI.DTOs.ForgetPassword
{
    public class ResetPasswordRequestDto
    {
        public string ResetToken { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
