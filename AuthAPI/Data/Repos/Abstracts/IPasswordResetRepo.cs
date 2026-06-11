using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Abstracts
{
    public interface IPasswordResetRepo
    {
        Task CreateOtpAsync(PasswordResetOtpEntity entity);
        Task<PasswordResetOtpEntity?> GetByResetTokenAsync(string resetToken);
        Task<PasswordResetOtpEntity?> GetLatestOtpByUserIdAsync(Guid userId);
        Task SaveChangesAsync();
    }
}