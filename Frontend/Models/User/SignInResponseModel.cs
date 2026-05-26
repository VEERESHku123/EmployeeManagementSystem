using Frontend.Models.Employee;

namespace Frontend.Models.User
{
    public class SignInResponseModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public AuthResponse AuthResponse { get; set; }
    }
}
