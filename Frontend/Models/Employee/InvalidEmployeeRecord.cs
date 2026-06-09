namespace Frontend.Models.Employee
{
    public class InvalidEmployeeRecord
    {
        public EmployeeUploadModel Employee { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
