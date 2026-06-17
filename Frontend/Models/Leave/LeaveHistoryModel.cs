namespace Frontend.Models.Leave
{
    public class LeaveHistoryModel
    {
        public int LeaveRequestId { get; set; }

        public string LeaveTypeName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public string? Reason { get; set; }

        public string Status { get; set; }

        public string? ManagerRemark { get; set; }

        public string? ApprovedByName { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
