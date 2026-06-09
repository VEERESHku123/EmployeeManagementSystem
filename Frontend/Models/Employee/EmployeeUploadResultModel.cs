namespace Frontend.Models.Employee
{
    public class EmployeeUploadResultModel
    {
        public int SuccessCount { get; set; }

        public int FailedCount { get; set; }

        public List<string> InsertedEmployeeIds { get; set; } = new();

        public string? InvalidFileName { get; set; }
        public List<InvalidEmployeeRecord> InvalidEmployeeRecords { get; set; } = new();
    }
}
