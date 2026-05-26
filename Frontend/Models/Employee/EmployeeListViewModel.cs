namespace Frontend.Models.Employee
{
    public class EmployeeListViewModel
    {
        public List<EmployeeModel> Employees { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int StatusCode { get; set; }

        public string Search { get; set; }
    }
}
