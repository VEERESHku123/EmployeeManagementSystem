using Frontend.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Frontend.Models.Employee
{
    public class EmployeeModel
    {
        [Required]
        [Length(10, 11, ErrorMessage = "Employee ID should contain 10 to 11 characters.")]
        [Remote("IsEmployeeIdAvailable", "Employee", ErrorMessage = "Employee ID already exists")]
        public required string EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name cannot be empty and must not exceed 50 characters.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid mobile number")]
        [Remote("IsPhoneAvailable", "Employee", ErrorMessage = "Phone number already exists")]
        public required string PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Personal email must end with @gmail.com")]
        public string? PersonalEmail { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@noventiqai\.com$", ErrorMessage = "Company email must end with @noventiqai.com")]
        public required string CompanyEmail { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public required DateOnly DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public DateOnly HiredDate { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;
        
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Department")]
        public int DepartmentId { get; set; }

        [StringLength(50, ErrorMessage = "Manager Id too long")]
        public string? ManagerId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}