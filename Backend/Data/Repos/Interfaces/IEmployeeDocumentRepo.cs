using Backend.Data.Entitys;

namespace Backend.Data.Repos.Interfaces
{
    public interface IEmployeeDocumentRepo
    {
        Task<List<DocumentCategoryEntity>> GetAllDocumentCategoriesAsync();
        Task<List<DocumentTypeEntity>> GetAllDocumentTypesAsync();
        Task SaveDocumentsAsync(List<EmployeeDocumentEntity> documents);
        Task<bool> DeleteDocumentAsync(Guid documentId);
        Task<EmployeeDocumentEntity?> GetDocumentByIdAsync(Guid documentId);
        Task<List<EmployeeDocumentEntity>> GetEmployeeDocuments(string employeeId);
    }
}