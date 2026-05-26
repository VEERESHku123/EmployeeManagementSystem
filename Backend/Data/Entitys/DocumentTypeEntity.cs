using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entitys
{
    public class DocumentTypeEntity
    {
        [Key]
        [Column("document_type_id")]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("document_name")]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(DocumentCategory))]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("is_mandatory")]
        public bool? IsMandatory { get; set; } = false;


        [InverseProperty(nameof(DocumentCategoryEntity.DocumentTypes))]
        public virtual DocumentCategoryEntity DocumentCategory { get; set; }
    }
}
