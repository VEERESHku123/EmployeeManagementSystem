using Backend.Data.Entitys;
using Backend.DTOs;
using Backend.DTOs.EmployeeDocument;

namespace Backend.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes();
        Task<ApiResponse<string>> UploadEmployeeDocumentsAsync(UploadEmployeeDocumentsModel model, string employeeId);
        Task<ApiResponse<string>> DeleteEmployeeDocumentAsync(Guid documentId);
        Task<ApiResponse<List<EmployeeDocumentDto>>> GetEmployeeDocuments(string employeeId);
    }
}