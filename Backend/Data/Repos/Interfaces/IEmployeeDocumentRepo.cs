using Backend.Data.Entitys;

namespace Backend.Data.Repos.Interfaces
{
    public interface IEmployeeDocumentRepo
    {
        Task<List<DocumentCategoryEntity>> GetAllDocumentCategories();
        Task<List<DocumentTypeEntity>> GetAllDocumentTypes();
    }
}