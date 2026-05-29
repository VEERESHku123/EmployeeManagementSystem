using Backend.Data.Entitys;

namespace Backend.Data.Repos.Interfaces
{
    public interface IEmployeeDocumentRepo
    {
        Task<List<DocumentCategoryEntity>> GetAllDocumentCategoriesAsync();
        Task<List<DocumentTypeEntity>> GetAllDocumentTypesAsync();
        Task<bool> SaveDocumentAsync(EmployeeDocumentEntity document);
        Task<bool> DeleteDocumentAsync(Guid documentId);
        Task<EmployeeDocumentEntity?> GetDocumentByIdAsync(Guid documentId);
        Task<List<EmployeeDocumentEntity>> GetEmployeeDocuments(string employeeId);
        Task<List<EmployeeDocumentEntity>>GetEmployeeDocumentsAsync(string employeeId);
    }
}