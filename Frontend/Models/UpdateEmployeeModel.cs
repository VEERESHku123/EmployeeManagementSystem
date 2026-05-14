using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class UpdateEmployeeModel
    {
        [Required]
        public required string EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be 2–50 characters")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be 2–50 characters")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [StringLength(50, ErrorMessage = "Job title can't exceed 50 characters")]
        public required string JobTitle { get; set; }
    }
}
