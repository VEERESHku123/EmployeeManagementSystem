using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IManagerApiService
    {
        Task<ApiResponse<List<ManagerModel>>> SendAllManagers();
    }
}