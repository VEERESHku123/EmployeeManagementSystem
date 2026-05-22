namespace Frontend.Models
{
    public class SignInResponseModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public AuthResponse AuthResponse { get; set; }
    }
}
