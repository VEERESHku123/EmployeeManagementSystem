using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities
{
    [Table("LeaveBalances")]
    public class LeaveBalanceEntity
    {
        [Key]
        [Column("balance_id")]
        public int BalanceId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [Required]
        [Column("leave_type_id")]
        public int LeaveTypeId { get; set; }

        [Required]
        [Column("total_leaves")]
        public int TotalLeaves { get; set; }

        [Column("used_leaves")]
        public int UsedLeaves { get; set; } = 0;

        // Navigation Properties

        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveTypeEntity LeaveType { get; set; }
    }
}
