using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<List<DepartmentModel>> GetAllDepartments();
    }
}