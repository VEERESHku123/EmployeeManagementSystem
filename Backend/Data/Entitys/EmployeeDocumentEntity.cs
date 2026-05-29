using Backend.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entitys
{
    [Table("EmployeeDocuments")]
    public class EmployeeDocumentEntity
    {
        [Key]
        [Column("document_id")]
        public int DocumentId { get; set; }

        [Required]
        [Column("employee_id")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [Column("document_type_id")]
        public int DocumentTypeId { get; set; }

        [Required]
        [Column("blob_name")]
        [StringLength(500)]
        public string BlobName { get; set; } = string.Empty;

        [Column("uploaded_date")]
        public DateTime UploadedDate { get; set; }

        [Column("verification_status")]
        [StringLength(50)]
        public string VerificationStatus { get; set; } = "Pending";

        [Column("remarks")]
        [StringLength(500)]
        public string? Remarks { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(EmployeeEntity.EmployeeDocuments))]
        public virtual EmployeeEntity? Employee { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(DocumentTypeEntity.EmployeeDocuments))]
        public virtual DocumentTypeEntity? DocumentType { get; set; }
    }
}