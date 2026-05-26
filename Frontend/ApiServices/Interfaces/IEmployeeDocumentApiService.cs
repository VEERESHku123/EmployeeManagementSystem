using Frontend.Models.Common;
using Frontend.Models.EmployeeDocument;

namespace Frontend.ApiServices.Interfaces
{
    public interface IEmployeeDocumentApiService
    {
        Task<ApiResponse<List<DocumentCategoryModel>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeModel>>> GetAllDocumentTypes();
    }
}