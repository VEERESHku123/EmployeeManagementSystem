using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities
{
    [Table("LeaveTypes")]
    public class LeaveTypeEntity
    {
        [Key]
        [Column("leave_type_id")]
        public int LeaveTypeId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("leave_type_name")]
        public string LeaveTypeName { get; set; }

        [Required]
        [Column("max_days_per_year")]
        public int MaxDaysPerYear { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<LeaveRequestEntity> LeaveRequests { get; set; } = new List<LeaveRequestEntity>();

        public virtual ICollection<LeaveBalanceEntity> LeaveBalances { get; set; } = new List<LeaveBalanceEntity>();
    }
}
