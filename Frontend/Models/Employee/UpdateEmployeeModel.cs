using Frontend.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Employee
{
    public class UpdateEmployeeModel
    {
       
        public required string EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be 2–50 characters")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be 1–50 characters")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid mobile number")]
        [Remote("IsPhoneAvailable", "Employee", AdditionalFields = "EmployeeId", ErrorMessage = "Phone number already exists")]
        public required string PhoneNumber { get; set; }
        public required string CompanyEmail { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Personal email must end with @gmail.com")]
        public string? PersonalEmail { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public required DateOnly DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public DateOnly HiredDate { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }

        public int RoleId { get; set; }
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }
        public string? ManagerId { get; set; }
    }
}
