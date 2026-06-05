namespace Backend.DTOs.Employee
{
    public class EmployeeUploadResultDTO
    {
        public int SuccessCount { get; set; }

        public int FailedCount { get; set; }

        public List<string> InsertedEmployeeIds { get; set; } = new();

        public string? InvalidFileName { get; set; }
    }
}
