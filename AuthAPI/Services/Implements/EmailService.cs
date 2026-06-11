using AuthAPI.DTOs.ForgetPassword;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using AuthAPI.Services.Abstracts;


namespace AuthAPI.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            settings = options.Value;
        }

        public async Task SendOtpAsync(string email, string otp)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(settings.Email));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Password Reset OTP";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP is {otp}. Valid for 30 Seconds."
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                settings.Host,
                settings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(settings.Email, settings.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}
