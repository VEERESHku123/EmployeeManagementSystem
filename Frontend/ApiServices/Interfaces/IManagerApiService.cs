using Frontend.Models;

namespace Frontend.ApiServices.Interfaces
{
    public interface IManagerApiService
    {
        Task<List<ManagerModel>> SendAllManagers();
    }
}