using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<EmployeeEntity> CheckEmailExistsAsync(string email);
    }
}