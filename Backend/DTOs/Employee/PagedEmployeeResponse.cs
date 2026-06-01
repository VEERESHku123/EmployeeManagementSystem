namespace Backend.DTOs.Employee
{
    public class PagedEmployeeResponse
    {
        public List<EmployeeDTO> Employees { get; set; }
        public int TotalCount { get; set; }

        
    }
}
