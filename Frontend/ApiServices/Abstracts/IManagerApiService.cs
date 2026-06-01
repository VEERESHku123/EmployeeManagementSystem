using Frontend.Models.Common;
using Frontend.Models.Employee;

namespace Frontend.ApiServices.Abstracts
{
    public interface IManagerApiService
    {
        Task<ApiResponse<List<ManagerModel>>> SendAllManagers();
    }
}