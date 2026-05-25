
using Backend.Enums;

namespace Backend.Data.Models
{
    public class EmployeeEntity
    {
        public required String EmployeeId { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String PhoneNumber { get; set; }
        public required string PersonalEmail { get; set; }
        public required string CompanyEmail { get; set; }
        public required DateOnly DOB { get; set; }
        public Gender Gender { get; set; }
        public DateOnly HiredDate { get; set; }
        public required string Designation { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }
        public string ManagerId { get; set; }


        //Navigation
        public DepartmentEntity Department { get; set; }
        public ManagerEntity Manager { get; set; }
    }
}
