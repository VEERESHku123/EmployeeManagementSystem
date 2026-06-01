using Backend.Data.Entities;
using Backend.DTOs.Common;
using Backend.DTOs.EmployeeDocument;

namespace Backend.Services.Abstracts
{
    public interface IEmployeeDocumentService
    {
        Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes();
        Task<ApiResponse<bool>> SaveDocument(string employeeId, SaveDocumentRequest request);

        Task<ApiResponse< List<EmployeeDocumentDto>>> GetEmployeeDocumentsAsync(string employeeId);
        Task<ApiResponse<bool>> DeleteDocumentAsync(string employeeId,Guid documentId);
        Task<ApiResponse<bool>> UpdateDocumentAsync(string employeeId,Guid documentId,UpdateDocumentRequest request);
        Task<ApiResponse<List<PendingDocumentDto>>> GetPendingActionDocumentsAsync();
    }
}