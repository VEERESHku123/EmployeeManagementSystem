namespace Frontend.Models.Leave
{
    public class LeaveBalanceModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int TotalLeaves { get; set; }
        public int UsedLeaves { get; set; }
        public int AvailableLeaves { get; set; }
    }
}
