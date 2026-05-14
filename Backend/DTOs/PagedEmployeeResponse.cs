using Backend.Data.Models;

namespace Backend.DTOs
{
    public class PagedEmployeeResponse
    {
        public List<EmployeeDTO> Employees { get; set; }
        public int TotalCount { get; set; }

        
    }
}
