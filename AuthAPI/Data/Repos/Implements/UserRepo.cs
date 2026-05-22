using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Interfaces;
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

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await context.Users
                         .Include(u => u.Role)
                         .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task SaveRefreshToken(int userId, string refreshToken, DateTime expiry)
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

    }
}
