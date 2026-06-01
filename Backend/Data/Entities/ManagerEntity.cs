namespace Backend.Data.Entities
{
    public class ManagerEntity
    {
        public required string ManagerId { get; set; }
        public required string ManagerName { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; } = [];
    }
}
