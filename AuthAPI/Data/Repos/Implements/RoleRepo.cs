using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos.Implements
{
    public class RoleRepo : IRoleRepo
    {
        private readonly AppDbContext context;

        public RoleRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<RoleEntity> GetRoleById(int roleId)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<RoleEntity> GetRoleByName(string role)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);
        }
    }
}
