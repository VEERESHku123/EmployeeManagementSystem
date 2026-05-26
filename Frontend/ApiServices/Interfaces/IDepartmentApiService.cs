using Frontend.Models.Common;
using Frontend.Models.Employee;

namespace Frontend.ApiServices.Interfaces
{
    public interface IDepartmentApiService
    {
        Task<ApiResponse<List<DepartmentModel>>> GetAllDepartments();
    }
}