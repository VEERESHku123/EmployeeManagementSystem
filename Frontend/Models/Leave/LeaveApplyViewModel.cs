namespace Frontend.Models.Leave
{
    public class LeaveApplyViewModel
    {
        public ApplyLeaveModel ApplyLeave { get; set; } = new();
        public List<LeaveBalanceModel> LeaveBalances { get; set; } = new();

        public List<LeaveHistoryModel> LeaveHistory { get; set; } = new();
    }
}
