using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Abstracts
{
    public interface IEmployeeRepo
    {
        Task<EmployeeEntity> CheckEmailExistsAsync(string email);
    }
}