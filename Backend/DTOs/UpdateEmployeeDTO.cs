using Backend.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class UpdateEmployeeDTO
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
        public required string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public required DateOnly DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public DateOnly HiredDate { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [StringLength(50)]
        public required string Designation { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;

        public int DepartmentId { get; set; }
        public string? ManagerId { get; set; }

    }
}

   