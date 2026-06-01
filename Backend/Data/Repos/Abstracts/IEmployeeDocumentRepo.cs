using Backend.Data.Entities;
using Backend.DTOs.EmployeeDocument;

namespace Backend.Data.Repos.Abstracts
{
    public interface IEmployeeDocumentRepo
    {
        Task<List<DocumentCategoryEntity>> GetAllDocumentCategoriesAsync();
        Task<List<DocumentTypeEntity>> GetAllDocumentTypesAsync();
        Task<bool> SaveDocumentAsync(EmployeeDocumentEntity document);
        Task<bool> DeleteDocumentAsync(Guid documentId);
        Task<EmployeeDocumentEntity?> GetDocumentByIdAsync(Guid documentId);
        Task<List<EmployeeDocumentEntity>>GetEmployeeDocumentsAsync(string employeeId);

        Task<EmployeeDocumentEntity?> GetDocumentAsync(string employeeId,Guid documentId);

        Task<bool> DeleteAsync(EmployeeDocumentEntity document);
        Task<bool> UpdateDocumentAsync(EmployeeDocumentEntity document, string blobName);
        Task<List<PendingDocumentDto>> GetPendingActionDocumentsAsync();
    }
}