using AuthAPI.Data.Entitys;
using AuthAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;
using System.Reflection;

namespace AuthAPI.Data.Context
{
    public class EmployeeDbContext : DbContext
    {
        

        public DbSet<EmployeeEntity> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions options) : base(options)
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
                .Property<string>(e => e.JobTitle)
               .HasColumnName("job_title")
               .HasColumnType("varchar(100)")
               .IsRequired();

            employeeBuilder
                .Property<decimal>(e => e.Salary)
               .HasColumnName("salary")
               .HasColumnType("decimal(18,2)")
               .IsRequired()
               .HasDefaultValue(0);

            employeeBuilder
                .Property<bool>(e => e.Status)
                .HasColumnName("status")
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
        }


    }
}
