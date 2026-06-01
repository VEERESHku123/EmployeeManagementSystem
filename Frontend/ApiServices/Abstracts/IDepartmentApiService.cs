using Frontend.Models.Common;
using Frontend.Models.Employee;

namespace Frontend.ApiServices.Abstracts
{
    public interface IDepartmentApiService
    {
        Task<ApiResponse<List<DepartmentModel>>> GetAllDepartments();
    }
}