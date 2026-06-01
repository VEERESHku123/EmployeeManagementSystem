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
        [Phone]
        public required string PhoneNumber { get; set; }


        [Column("personal_email")]
        [RegularExpression(
            @"^[a-zA-Z0-9._%+-]+@gmail\.com$",
            ErrorMessage = "Personal email must end with @gmail.com"
        )]
        [StringLength(150)]
        public string? PersonalEmail { get; set; }


        [Required]
        [Column("company_email")]
        [RegularExpression(
            @"^[a-zA-Z0-9._%+-]+@noventiqai\.com$",
            ErrorMessage = "Company email must end with @noventiqai.com"
        )]
        [StringLength(150)]
        public required string CompanyEmail { get; set; }


        [Required]
        [Column("dob")]
        public required DateOnly DOB { get; set; }


        [Required]
        [Column("gender")]
        public string Gender { get; set; }


        [Required]
        [Column("hired_date")]
        public DateOnly HiredDate { get; set; }


        [Required]
        [Column("designation")]
        [StringLength(100)]
        public required string Designation { get; set; }


        [Column("salary")]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; } = 0;


        [Column("is_active")]
        public bool IsActive { get; set; } = true;


        [Required]
        [Column("department_id")]
        public int DepartmentId { get; set; }


        [Column("manager_id")]
        [StringLength(50)]
        public string? ManagerId { get; set; }


        // Navigation Properties
        public UserEntity User { get; set; }

        // You should add:
        // public DepartmentEntity Department { get; set; }
        // public ManagerEntity Manager { get; set; }
    }
}