using AuthAPI.Data.Context;
using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos.Implements
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
                return await context.Employees.FirstOrDefaultAsync(e => e.CompanyEmail == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }
}
