namespace Backend.DTOs.EmployeeLeave
{
    public class LeaveBalanceDto
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int TotalLeaves { get; set; }
        public int UsedLeaves { get; set; }
        public int AvailableLeaves { get; set; }
    }
}
