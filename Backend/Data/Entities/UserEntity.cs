using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Entities.User
{
    [Table("user")]
    public class UserEntity
    {
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("password_hash")]
        public required string PasswordHash { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public RoleEntity Role { get; set; }


        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(EmployeeEntity.User))]
        public virtual EmployeeEntity Employee { get; set; }



    }
}
