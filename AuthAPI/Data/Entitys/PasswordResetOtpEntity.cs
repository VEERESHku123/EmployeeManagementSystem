using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthAPI.Data.Entitys
{
    [Table("PasswordResetOtp")]
    public class PasswordResetOtpEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("otp_code")]
        [StringLength(20)]
        public string OtpCode { get; set; } = null!;

        [Required]
        [Column("otp_expires_at")]
        public DateTime OtpExpiresAt { get; set; }

        [Column("is_otp_used")]
        public bool IsOtpUsed { get; set; } = false;

        [Column("reset_token")]
        [StringLength(500)]
        public string? ResetToken { get; set; }

        [Column("reset_token_expires_at")]
        public DateTime? ResetTokenExpiresAt { get; set; }

        [Column("is_reset_token_used")]
        public bool IsResetTokenUsed { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; } = null!;
    }
}