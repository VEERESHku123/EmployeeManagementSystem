using AuthAPI.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos
{
    public class EmployeeRepo
    {
        private readonly EmployeeDbContext context;

        public EmployeeRepo(EmployeeDbContext context)
        {
            this.context = context;
        }

        public async Task<string> CheckEmailExistsAsync(string email)
        {
            try
            {
                var result = await context.Employees.SingleOrDefaultAsync(e => e.Email == email);
                if (result != null)
                    return result.Role.ToString();
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
