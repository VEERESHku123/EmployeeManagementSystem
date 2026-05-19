using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
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
        [Required]
        [EmailAddress(ErrorMessage = "Enter Valid EMail")]
        public required string Email { get; set; }
        [Required]
        public required DateOnly DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly HiredDate { get; set; }
        [Required]
        public required string JobTitle { get; set; }
        [Range(0, 10000000)]
        public decimal Salary { get; set; }

        public Role Role { get; set; }

        public int DepartmentId { get; set; }
        public string? ManagerId { get; set; }
    }
}
