using AuthAPI.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repos
{
    public class UserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<(bool isExsist, string message)> UserExsist(string email, string password)
        {
            try
            {
                var user = await context.USers.FirstOrDefaultAsync(u => u.Email == email);
                if(user == null)
                {
                    return (false, "Invalid Email");
                }
                return (true, "");

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
