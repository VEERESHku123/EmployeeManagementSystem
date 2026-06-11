using AuthAPI.Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Context
{
    public class AppDbContext : DbContext
    {


        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<PasswordResetOtpEntity> PasswordResetOtps { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.Gender)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }


    }
}
