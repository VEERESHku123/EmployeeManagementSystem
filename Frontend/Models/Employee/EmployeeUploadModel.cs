using Frontend.Enums;

namespace Frontend.Models.Employee
{
    public class EmployeeUploadModel
    {
        public string EmployeeId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? PersonalEmail { get; set; }

        public string CompanyEmail { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public DateOnly HiredDate { get; set; }

        public decimal Salary { get; set; }

        public string GenderText { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string DesignationName { get; set; } = string.Empty;

        public string? ManagerName { get; set; }

        // Resolved entities after validation
        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public string? ManagerId { get; set; }

        public Gender Gender { get; set; }
    }
}
