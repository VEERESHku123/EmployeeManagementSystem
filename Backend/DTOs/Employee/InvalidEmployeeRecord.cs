namespace Backend.DTOs.Employee
{
    public class InvalidEmployeeRecord
    {
        public EmployeeUploadDTO Employee { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
