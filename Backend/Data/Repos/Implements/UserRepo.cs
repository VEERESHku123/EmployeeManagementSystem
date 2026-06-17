using Backend.Data.Context;
using Backend.Data.Entities;
using Backend.Data.Entities.User;
using Backend.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repos.Implements
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddUser(UserEntity userEntity)
        {
            await context.Users.AddAsync(userEntity);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task BulkInsertUserAsync(List<UserEntity> users)
        {
            try
            {
                await context.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        #region Role Section

        public async Task<List<RoleEntity>> GetAllRoles()
        {
            return await context.Roles.ToListAsync();
        }

        public async Task<bool> UpdateUserRole(string employeeId, int roleId)
        {
            var found = await context.Users.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            if(found != null)
            {
                found.RoleId = roleId;

                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }

        #endregion
    }
}
