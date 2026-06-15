using AuthAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthAPI.Data.Entitys
{
    [Table("Employees")]
    public class EmployeeEntity
    {
        [Key]
        [Column("employee_id")]
        [StringLength(50)]
        public required string EmployeeId { get; set; }

        [Required]
        [Column("first_name")]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [Column("phone_number")]
        [StringLength(20)]
        public required string PhoneNumber { get; set; }

        [Column("personal_email")]
        [StringLength(150)]
        public string? PersonalEmail { get; set; }

        [Required]
        [Column("company_email")]
        [StringLength(150)]
        public required string CompanyEmail { get; set; }

        [Required]
        [Column("dob", TypeName = "date")]
        public required DateOnly DOB { get; set; }

        [Required]
        [Column("gender")]
        public Gender Gender { get; set; }

        [Required]
        [Column("hired_date", TypeName = "date")]
        public DateOnly HiredDate { get; set; }

        [Column("salary", TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("manager_id")]
        [StringLength(50)]
        public string? ManagerId { get; set; }

        [Column("designation_id")]
        public int DesignationId { get; set; }

        // Navigation Properties

        [InverseProperty(nameof(UserEntity.Employee))]
        public virtual UserEntity? User { get; set; }
    }
}