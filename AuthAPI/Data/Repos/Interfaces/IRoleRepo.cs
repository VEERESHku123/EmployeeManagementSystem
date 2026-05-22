using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Interfaces
{
    public interface IRoleRepo
    {
        Task<RoleEntity> GetRoleByName(string role);
    }
}