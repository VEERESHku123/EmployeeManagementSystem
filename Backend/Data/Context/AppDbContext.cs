using Backend.Data.Models;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<ManagerEntity> Managers { get; set; }  

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
                .HasColumnType("varchar(50)")
                .IsRequired();

            employeeBuilder.Property<string>(e => e.PhoneNumber)
               .HasColumnName("phone_number")
               .HasColumnType("varchar(20)")
               .IsRequired();

            employeeBuilder
                .HasIndex(e => e.PhoneNumber)
                .IsUnique();


            employeeBuilder
                .Property<string>(e => e.PersonalEmail)
               .HasColumnName("personal_email")
               .HasColumnType("varchar(150)")
               .IsRequired();

            employeeBuilder
                .HasIndex(e => e.PersonalEmail)
                .IsUnique();

            employeeBuilder
                .Property<string>(e => e.CompanyEmail)
               .HasColumnName("company_email")
               .HasColumnType("varchar(150)")
               .IsRequired();

            employeeBuilder
                .HasIndex(e => e.CompanyEmail)
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

            employeeBuilder
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            employeeBuilder
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Department Builder
            EntityTypeBuilder<DepartmentEntity> departmentBuilder = modelBuilder.Entity<DepartmentEntity>();

            departmentBuilder
                .ToTable("department")
                .HasKey(d => d.DepartmentId);

            departmentBuilder
                .Property<int>(d => d.DepartmentId)
                .HasColumnName("department_id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(100, 1);

            departmentBuilder
                .Property<string>(d => d.DepartmentName)
                .HasColumnName("department_name")
                .HasColumnType("varchar(50)")
                .IsRequired();
            #endregion

            #region Manager Builder
            EntityTypeBuilder<ManagerEntity> managerBuilder = modelBuilder.Entity<ManagerEntity>();

            managerBuilder
                .ToTable("managers")
                .HasKey(m => m.ManagerId);

            managerBuilder.Property<string>(m => m.ManagerId)
                .HasColumnName("manager_id")
                .HasColumnType("varchar(50)")
                .IsRequired();

            managerBuilder
                .HasIndex(m => m.ManagerId)
                .IsUnique();


            managerBuilder.Property<string>(m => m.ManagerName)
                .HasColumnName("manager_name")
                .HasColumnType("varchar(50)")
                .IsRequired();

            #endregion
        }
    }
}
