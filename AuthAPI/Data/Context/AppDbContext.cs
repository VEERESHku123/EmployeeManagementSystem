using AuthAPI.Data.Entitys;
using AuthAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthAPI.Data.Context
{
    public class AppDbContext : DbContext
    {


        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<RoleEntiry> Roles { get; set; }
        public DbSet<UserEntity> USers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            #region Employee Builder
            EntityTypeBuilder<EmployeeEntity> employeeBuilder = modelBuilder.Entity<EmployeeEntity>();

            employeeBuilder
                .ToTable("Employees")
                .HasKey(e => e.EmployeeId);

            employeeBuilder.Property<string>(e => e.EmployeeId)
                .HasColumnName("employee_id")
                .HasColumnType("varchar(50)")
                .IsRequired();

            employeeBuilder
                .HasIndex(e => e.EmployeeId)
                .IsUnique();

            employeeBuilder.Property<string>(e => e.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(50)")
                .IsRequired();

            employeeBuilder.Property<string>(e => e.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(500)")
                .IsRequired();

            employeeBuilder.Property<string>(e => e.PhoneNumber)
               .HasColumnName("phone_number")
               .HasColumnType("varchar(20)")
               .IsRequired();

            employeeBuilder
                .HasIndex(e => e.PhoneNumber)
                .IsUnique();


            employeeBuilder
                .Property<string>(e => e.Email)
               .HasColumnName("email")
               .HasColumnType("varchar(150)")
               .IsRequired();

            employeeBuilder
                .HasIndex(e => e.Email)
                .IsUnique();


            employeeBuilder
                .Property<DateOnly>(e => e.DOB)
               .HasColumnName("dob")
               .HasColumnType("date")
               .IsRequired();

            employeeBuilder
                .Property<Gender>(e => e.Gender)
                .HasColumnName("gender")
                .HasColumnType("varchar(20)")
                .IsRequired();

            employeeBuilder
                .Property<Role>(e => e.Role)
                .HasColumnType("varchar(20)")
                .HasColumnName("role-type")
                .IsRequired();

            employeeBuilder
                .Property<DateOnly>(e => e.HiredDate)
                .HasColumnName("hired_date")
                .HasColumnType("date")
                .IsRequired();

            employeeBuilder
                .Property<string>(e => e.Designation)
               .HasColumnName("designation")
               .HasColumnType("varchar(100)")
               .IsRequired();

            employeeBuilder
                .Property<decimal>(e => e.Salary)
               .HasColumnName("salary")
               .HasColumnType("decimal(18,2)")
               .IsRequired()
               .HasDefaultValue(0);

            employeeBuilder
                .Property<bool>(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("bit")
                .HasDefaultValue(true);

            employeeBuilder
                .Property<int>(e => e.DepartmentId)
                .HasColumnName("department_id")
                .HasColumnType("int")
                .IsRequired();

            employeeBuilder
                .Property<string>(e => e.ManagerId)
                .HasColumnName("manager_id")
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            #endregion

            #region RoleBuilder

            EntityTypeBuilder<RoleEntiry> roleBuilder = modelBuilder.Entity<RoleEntiry>();

            roleBuilder
                .ToTable("role")
                .HasKey(r => r.RoleId);

            roleBuilder
                .Property<int>(r => r.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(100, 1);

            roleBuilder
                .Property<string>(r => r.RoleName)
                .HasColumnName("role_name")
                .HasColumnType("varchar(30)")
                .IsRequired();

            #endregion

            #region UserBuilder

            EntityTypeBuilder<UserEntity> userBuilder =  modelBuilder.Entity<UserEntity>();

            userBuilder
                .ToTable("user")
                .HasKey(u => u.UserId);

            userBuilder
                .Property<int>(u => u.UserId)
                .HasColumnName("user_id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1);

            userBuilder
                .Property<string>(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(60)")
                .IsRequired();

            userBuilder
                .Property<string>(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("nvarchar(500)")
                .IsRequired();

            userBuilder
                .Property<int>(u => u.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("int")
                .IsRequired();

            userBuilder
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

        }


    }
}
