using AuthAPI.Data.Entitys;

namespace AuthAPI.Data.Repos.Implements
{
    public interface IEmployeeRepo
    {
        Task<EmployeeEntity> CheckEmailExistsAsync(string email);
    }
}