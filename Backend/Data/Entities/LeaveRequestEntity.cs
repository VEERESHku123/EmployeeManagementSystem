using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities
{
    [Table("leave_requests")]
    public class LeaveRequestEntity
    {
        [Key]
        [Column("leave_request_id")]
        public int LeaveRequestId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [Required]
        [Column("leave_type_id")]
        public int LeaveTypeId { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("total_days")]
        public int TotalDays { get; set; }

        [StringLength(500)]
        [Column("reason")]
        public string? Reason { get; set; }

        [Required]
        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; } = "Pending"; 
        
        [StringLength(50)]
        [Column("approved_by_employee_id")]
        public string? ApprovedByEmployeeId { get; set; }

        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties

        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveTypeEntity LeaveType { get; set; }

        [ForeignKey(nameof(ApprovedByEmployeeId))]
        public virtual EmployeeEntity? ApprovedByEmployee { get; set; }
    }
}
