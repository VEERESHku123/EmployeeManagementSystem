using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.User
{
    public class ActivateAccountModel
    {

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|noventiqai\.com)$",
                                ErrorMessage = "Use Gmail or company email (@noventiqai.com)")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
        ErrorMessage =
        "Password must contain uppercase, lowercase, number and special char")]
        public string Password { get; set; }


        [Compare("Password",
        ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
