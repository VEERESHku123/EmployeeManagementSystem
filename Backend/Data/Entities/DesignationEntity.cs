using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities
{
    [Table("Designations")]
    public class DesignationEntity
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DesignationName { get; set; } = string.Empty;

        public List<EmployeeEntity> Employees { get; set; } = [];
    }
}