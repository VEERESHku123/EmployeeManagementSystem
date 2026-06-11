using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using AuthAPI.DTOs.Common;
using AuthAPI.Services.Abstracts;

namespace AuthAPI.Services.Implements
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepo userRepo;
        private readonly IPasswordResetRepo passwordResetRepo;
        private readonly IEmailService emailService;
        private readonly ILogger<PasswordResetService> logger;

        public PasswordResetService(IUserRepo userRepo, IPasswordResetRepo passwordResetRepo, IEmailService emailService, ILogger<PasswordResetService> logger)
        {
            this.userRepo = userRepo;
            this.passwordResetRepo = passwordResetRepo;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task<ApiResponse<Object>> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await userRepo.GetUserByEmail(email);

                if (user == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var otp = Random.Shared.Next(100000, 999999).ToString();

                var otpEntity = new PasswordResetOtpEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = user.UserId,
                    OtpCode = otp,
                    OtpExpiresAt = DateTime.UtcNow.AddSeconds(60),
                    CreatedAt = DateTime.UtcNow
                };

                await passwordResetRepo.CreateOtpAsync(otpEntity);
                await passwordResetRepo.SaveChangesAsync();

                await emailService.SendOtpAsync(user.Email, otp);

                logger.LogInformation("Password reset OTP sent to user {UserId}", user.UserId);

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "OTP sent successfully",
                    Data = new
                    {
                        ExpiresAt = otpEntity.OtpExpiresAt
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while sending password reset OTP for {Email}",
                    email);

                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to send OTP",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<string>> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                var user = await userRepo.GetUserByEmail(email);

                if (user == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var otpRecord = await passwordResetRepo.GetLatestOtpByUserIdAsync(user.UserId);

                if (otpRecord == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "OTP not found"
                    };
                }

                if (otpRecord.IsOtpUsed)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "OTP already used"
                    };
                }

                if (otpRecord.OtpExpiresAt < DateTime.UtcNow)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "OTP expired"
                    };
                }

                if (otpRecord.OtpCode != otp)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Invalid OTP"
                    };
                }

                otpRecord.IsOtpUsed = true;
                otpRecord.ResetToken = Guid.NewGuid().ToString();
                otpRecord.ResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);

                await passwordResetRepo.SaveChangesAsync();

                logger.LogInformation("OTP verified successfully for user {UserId}", user.UserId);

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "OTP verified successfully",
                    Data = otpRecord.ResetToken
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while verifying OTP for {Email}",
                    email);

                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "OTP verification failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(string resetToken, string newPassword)
        {
            try
            {
                var otpRecord = await passwordResetRepo.GetByResetTokenAsync(resetToken);

                if (otpRecord == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid token"
                    };
                }

                if (otpRecord.IsResetTokenUsed)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Token already used"
                    };
                }

                if (otpRecord.ResetTokenExpiresAt < DateTime.UtcNow)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Token expired"
                    };
                }

                otpRecord.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                otpRecord.IsResetTokenUsed = true;

                await passwordResetRepo.SaveChangesAsync();

                logger.LogInformation("Password reset successful for user {UserId}", otpRecord.UserId);

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Password reset successful"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while resetting password using token");

                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Password reset failed",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}
