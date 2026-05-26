using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.User
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|noventiqai\.com)$",
                                ErrorMessage = "Use Gmail or company email (@noventiqai.com)")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
