using Frontend.Models;
using Frontend.Models.Common;
using Frontend.Models.EmployeeDocument;

namespace Frontend.ApiServices.Interfaces
{
    public interface IEmployeeDocumentApiService
    {
        Task<ApiResponse<List<DocumentCategoryModel>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeModel>>> GetAllDocumentTypes();
        Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocuments();
        Task<ApiResponse<UploadSasResponse>> GenerateUploadSasAsync(GenerateUploadSasRequest model);
        Task<ApiResponse<bool>> UploadDocumentsAsync(int documentTypeId, IFormFile file);

        Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocumentsAsync();

    }
}