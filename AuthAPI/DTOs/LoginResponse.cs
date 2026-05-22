namespace AuthAPI.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public AuthResponse AuthResponse { get; set; }
    }
}
