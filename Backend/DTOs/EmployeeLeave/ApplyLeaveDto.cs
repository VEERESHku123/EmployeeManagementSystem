using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.EmployeeLeave
{
    public class ApplyLeaveDto
    {
        public string? EmployeeId { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }
    }
}
