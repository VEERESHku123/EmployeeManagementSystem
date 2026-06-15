using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Leave
{
    public class ApplyLeaveDto
    {
        [Required]
        public string EmployeeId { get; set; }

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
