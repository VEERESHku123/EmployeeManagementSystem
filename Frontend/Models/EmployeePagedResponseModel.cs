namespace Frontend.Models
{
    public class EmployeePagedResponseModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
