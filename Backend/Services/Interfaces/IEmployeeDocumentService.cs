using Backend.Data.Entitys;
using Backend.DTOs;
using Backend.DTOs.EmployeeDocument;

namespace Backend.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes();
        Task<ApiResponse<bool>> SaveDocument(string employeeId, SaveDocumentRequest request);

        Task<ApiResponse< List<EmployeeDocumentDto>>> GetEmployeeDocumentsAsync(string employeeId);
    }
}