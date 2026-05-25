using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<ApiResponse<List<DepartmentModel>>> GetAllDepartments();
    }
}