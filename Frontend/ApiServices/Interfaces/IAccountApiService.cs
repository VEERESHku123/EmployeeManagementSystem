using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IAccountApiService
    {
        Task<AuthResponse> GetJwtToken(string email, string name);
    }
}