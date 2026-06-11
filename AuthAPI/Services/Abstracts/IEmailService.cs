namespace AuthAPI.Services.Abstracts
{
    public interface IEmailService
    {
        Task SendOtpAsync(string email, string otp);
    }
}