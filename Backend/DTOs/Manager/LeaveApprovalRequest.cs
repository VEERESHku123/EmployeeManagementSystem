using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Manager
{
    public class LeaveApprovalRequest
    {
        [Required]
        public int LeaveRequestId { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty; // Approved or Rejected

        public string? ManagerRemark { get; set; }
    }
}
