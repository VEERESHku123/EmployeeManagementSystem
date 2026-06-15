namespace AuthAPI.DTOs.SigIn
{
    public class ActivateAccountDto
    {
        public string Email { get; set; }
        public string TemporaryPassword { get; set; }
        public string Password { get; set; }
    }
}
