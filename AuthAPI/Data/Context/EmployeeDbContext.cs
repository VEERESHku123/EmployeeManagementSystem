using AuthAPI.Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Context
{
    public class EmployeeDbContext : DbContext
    {
        

        public DbSet<EmployeeEntity> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
