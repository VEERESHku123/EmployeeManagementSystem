using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities
{
    public class DocumentCategoryEntity
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("category_name")]
        public string CategoryName { get; set; }


        [InverseProperty(nameof(DocumentTypeEntity.DocumentCategory))]
        public virtual ICollection<DocumentTypeEntity> DocumentTypes { get; set; }
        = new List<DocumentTypeEntity>();
    }
}
