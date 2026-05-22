using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<UserEntity?> GetUserByEmail(string email);
        Task<bool> AddUser(UserEntity user);
        Task SaveRefreshToken(int userId, string refreshToken, DateTime expiry);
    }
}