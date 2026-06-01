using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Abstracts
{
    public interface IRoleRepo
    {
        Task<RoleEntity> GetRoleByName(string role);
    }
}