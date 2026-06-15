using Backend.Data.Entities;
using Backend.Data.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
       

        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<DocumentCategoryEntity> DocumentCategories { get; set; }
        public DbSet<DocumentTypeEntity> DocumentTypes { get; set; }
        public DbSet<EmployeeDocumentEntity> EmployeeDocuments { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<DesignationEntity> Designations { get; set; }
        public DbSet<LeaveTypeEntity> LeaveTypes { get; set; }
        public DbSet<LeaveRequestEntity> LeaveRequests { get; set; }
        public DbSet<LeaveBalanceEntity> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeEntity>()
                    .Property(e => e.Gender)
                    .HasConversion<string>();

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

           
        }
    }
}
