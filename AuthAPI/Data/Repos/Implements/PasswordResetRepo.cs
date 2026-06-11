using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos.Implements
{
    public class PasswordResetRepo : IPasswordResetRepo
    {
        private readonly AppDbContext context;

        public PasswordResetRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateOtpAsync(PasswordResetOtpEntity entity)
        {
            await context.PasswordResetOtps.AddAsync(entity);
        }

        public async Task<PasswordResetOtpEntity?> GetLatestOtpByUserIdAsync(Guid userId)
        {
            return await context.PasswordResetOtps
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<PasswordResetOtpEntity?> GetByResetTokenAsync(string resetToken)
        {
            return await context.PasswordResetOtps
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ResetToken == resetToken);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
