using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [RegularExpression(
         @"^[a-zA-Z0-9._%+-]+@noventiai\.com$",
         ErrorMessage = "Use company email (@noventi.com)")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]

        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage =
            "Password must contain 8+ chars, uppercase, lowercase, number and special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
