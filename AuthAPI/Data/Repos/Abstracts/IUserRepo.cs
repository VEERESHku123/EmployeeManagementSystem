using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Abstracts
{
    public interface IUserRepo
    {
        Task<UserEntity?> GetUserByEmail(string email);
        Task<bool> AddUser(UserEntity user);
        Task SaveRefreshToken(Guid userId, string? refreshToken, DateTime? expiry);
        Task<UserEntity> GetByRefreshToken(string refreshToken);
        Task<UserEntity> GetUserByEmployeeId(string employeeId);
    }
}