using AuthAPI.DTOs.Common;

namespace AuthAPI.DTOs.SigIn
{
    public class SignInResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public AuthResponse AuthResponse { get; set; }
    }
}
