using AuthAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Data.Entitys
{
    public class EmployeeEntity
    {
        [Key]
        public required String EmployeeId { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required DateOnly DOB { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public DateOnly HiredDate { get; set; }
        public required string Designation { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; } = true;

        public int DepartmentId { get; set; }
        public string ManagerId { get; set; }
    }
}
