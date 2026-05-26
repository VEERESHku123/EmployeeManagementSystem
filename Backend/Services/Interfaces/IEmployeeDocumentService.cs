using Backend.Data.Entitys;
using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories();
        Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes();
    }
}