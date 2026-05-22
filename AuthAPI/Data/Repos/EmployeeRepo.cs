using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Implements;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly AppDbContext context;

        public EmployeeRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<EmployeeEntity> CheckEmailExistsAsync(string email)
        {
            try
            {
                return await context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
