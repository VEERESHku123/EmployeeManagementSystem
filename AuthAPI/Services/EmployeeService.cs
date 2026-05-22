using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos;

namespace AuthAPI.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepo employeeRepo;

        public EmployeeService(EmployeeRepo employeeRepo)
        {
            this.employeeRepo = employeeRepo;
        }


        public async Task<EmployeeEntity> CheckEmailExistsAsync(string email)
        {
            try
            {
                return await employeeRepo.CheckEmailExistsAsync(email);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
