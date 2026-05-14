namespace Backend.Data.Models
{
    public class DepartmentEntity
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; } = [];
    }
}
