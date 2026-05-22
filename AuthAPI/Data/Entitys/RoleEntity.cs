using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthAPI.Data.Entitys
{
    [Table("role")]
    public class RoleEntity
    {
        [Key]
        [Column("role_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [Column("role_name", TypeName = "varchar(30)")]
        public string RoleName { get; set; }

        public List<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
