using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Leave
{
    using System.ComponentModel.DataAnnotations;

    public class ApplyLeaveModel 
    {

        [Required(ErrorMessage = "Leave Type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid leave type.")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        public string Reason { get; set; }
    }
}
