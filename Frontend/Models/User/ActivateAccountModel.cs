using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.User
{
    public class ActivateAccountModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(
            @"^[a-zA-Z0-9._%+-]+@(gmail\.com|noventiqai\.com)$",
            ErrorMessage = "Use Gmail or company email (@noventiqai.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Temporary password is required")]
        public string TemporaryPassword { get; set; }

        [Required]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage =
            "Password must contain uppercase, lowercase, number and special character")]
        public string Password { get; set; }

        [Required]
        [Compare("Password",
            ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}