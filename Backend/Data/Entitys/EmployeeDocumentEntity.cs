using Backend.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Backend.Data.Entitys
{
    [Table("EmployeeDocuments")]
    public class EmployeeDocumentEntity
    {
        [Key]
        [Column("employee_document_id")]
        public Guid EmployeeDocumentId { get; set; }

        [Required]
        [Column("employee_id")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [Column("document_type_id")]
        public int DocumentTypeId { get; set; }

        [Required]
        [Column("file_name")]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [Column("file_url")]
        public string FileUrl { get; set; } = string.Empty;

        [Column("uploaded_date")]
        public DateTime? UploadedDate { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("verification_status")]
        [StringLength(20)]
        public string? VerificationStatus { get; set; } = "Pending";

        [Column("remarks")]
        [StringLength(500)]
        public string? Remarks { get; set; }

        // Navigation Properties

        [ForeignKey("EmployeeId")]
        [InverseProperty(nameof(EmployeeEntity.EmployeeDocuments))]
        public virtual EmployeeEntity? Employee { get; set; }

        [ForeignKey("DocumentTypeId")]
        [InverseProperty(nameof(DocumentTypeEntity.EmployeeDocuments))]
        public virtual DocumentTypeEntity? DocumentType { get; set; }
    }
}
