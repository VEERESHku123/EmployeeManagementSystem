using AuthAPI.Data.Entitys;

namespace AuthAPI.DTOs.SigIn
{
    public class SignInValidationResult
    {
        public EmployeeEntity? Employee { get; set; }

        public SignInResponse? Error { get; set; }

        public bool IsValid => Employee != null && Error == null;
    }
}
