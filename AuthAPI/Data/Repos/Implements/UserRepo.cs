using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos.Implements
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task SaveRefreshToken(Guid userId, string? refreshToken, DateTime? expiry)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return;

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiry;

            await context.SaveChangesAsync();
        }

        public async Task<bool> AddUser(UserEntity user)
        {
            await context.Users.AddAsync(user);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<UserEntity> GetByRefreshToken(string refreshToken)
        {
            try
            {
                var user = await context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserEntity> GetUserByEmployeeId(string employeeId)
        {
            return await context.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
        }
    }
}
