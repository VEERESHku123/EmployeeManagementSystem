using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        [Required]
        [Length(10, 11, ErrorMessage = "Employee ID should contain 10 to 11 characters.")]
        public required string EmployeeId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "First Name should at least have two letter")]
        public required string FirstName { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Last Name should at least have One letter")]
        public required string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}",ErrorMessage = "Enter Valid Mobile Number") ]
        public required string PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Personal email must end with @gmail.com")]
        public string? PersonalEmail { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@noventiqai\.com$", ErrorMessage = "Company email must end with @noventiqai.com")]
        public required string CompanyEmail { get; set; }

        [Required]
        public required DateOnly DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly HiredDate { get; set; }
        [Required]
        public required string Designation { get; set; }
        [Range(0, 10000000)]
        public decimal Salary { get; set; }

        public Role Role { get; set; }

        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public string? ManagerId { get; set; }
    }
}
