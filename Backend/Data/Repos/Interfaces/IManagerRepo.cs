using Backend.Data.Context;
using Backend.Data.Models;

namespace Backend.Data.Repos.Interfaces
{
    public interface IManagerRepo
    {
        Task<List<ManagerEntity>> GetAllAsync();
    }
}