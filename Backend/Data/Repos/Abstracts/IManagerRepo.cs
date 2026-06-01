using Backend.Data.Entities;

namespace Backend.Data.Repos.Abstracts
{
    public interface IManagerRepo
    {
        Task<List<ManagerEntity>> GetAllAsync();
    }
}