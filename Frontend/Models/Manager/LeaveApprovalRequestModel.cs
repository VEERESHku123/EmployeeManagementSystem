using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Manager
{
    public class LeaveApprovalRequestModel
    {
        [Required]
        public int LeaveRequestId { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty; // Approved or Rejected

        public string? ManagerRemark { get; set; }
    }
}
