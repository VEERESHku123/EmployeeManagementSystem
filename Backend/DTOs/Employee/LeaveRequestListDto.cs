namespace Backend.DTOs.Employee
{
    public class LeaveRequestListDto
    {
        public int LeaveRequestId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public string Status { get; set; }

        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
