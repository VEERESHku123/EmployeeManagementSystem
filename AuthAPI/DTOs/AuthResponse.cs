namespace AuthAPI.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

        public string RoleType { get; set; }
    }

}
