using Frontend.Enums;
using System.ComponentModel.DataAnnotations;
namespace Frontend.Models
{
    public class EmployeeModel
    {
        [Required]
        public required string EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid mobile number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public required DateOnly DOB { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        public DateOnly HiredDate { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [StringLength(50)]
        public required string JobTitle { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        public bool Status { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Department")]
        public int DepartmentId { get; set; }

        [StringLength(50, ErrorMessage = "Manager Id too long")]
        public string? ManagerId { get; set; }
    }
}