using Frontend.Models;
using Frontend.Models.Common;
using Frontend.Models.EmployeeDocument;

namespace Frontend.ApiServices.Abstracts
{
    public interface IEmployeeDocumentApiService
    {
        Task<ApiResponse<List<DocumentCategoryModel>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeModel>>> GetAllDocumentTypes();
        Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocuments();
        Task<ApiResponse<UploadSasResponse>> GenerateUploadSasAsync(GenerateUploadSasRequest model);
        Task<ApiResponse<bool>> UploadDocumentsAsync(int documentTypeId, IFormFile file);

        Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocumentsAsync(string? employeeId);

        Task<ApiResponse<bool>> DeleteDocumentAsync(string employeeId,Guid documentId);
        Task<ApiResponse<bool>> UpdateDocumentAsync(Guid documentId, int documentTypeId, IFormFile file, string? employeeId = null);

        Task<ApiResponse<List<PendingDocumentModel>>> GetPendingDocumentsAsync();

        Task<ApiResponse<bool>> ApproveDocumentAsync(string employeeId,Guid documentId,string? remarks);

        Task<ApiResponse<bool>> RejectDocumentAsync(string employeeId,Guid documentId,string remarks);

    }
}