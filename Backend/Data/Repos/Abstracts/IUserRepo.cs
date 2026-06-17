using Backend.Data.Entities;
using Backend.Data.Entities.User;

namespace Backend.Data.Repos.Abstracts
{
    public interface IUserRepo
    {
        Task<bool> AddUser(UserEntity userEntity);

        Task BulkInsertUserAsync(List<UserEntity> users);

        Task<List<RoleEntity>> GetAllRoles();
        Task<bool> UpdateUserRole(string employeeId, int roleId);
    }
}