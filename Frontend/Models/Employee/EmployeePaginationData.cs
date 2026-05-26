namespace Frontend.Models.Employee
{
    public class EmployeePaginationData
    {
        public List<EmployeeModel> Employees { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
