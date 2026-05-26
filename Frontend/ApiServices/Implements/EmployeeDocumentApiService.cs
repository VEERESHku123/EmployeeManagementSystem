using Frontend.ApiServices.Interfaces;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using Frontend.Models.EmployeeDocument;

namespace Frontend.ApiServices.Implements
{
    public class EmployeeDocumentApiService : BaseApiService, IEmployeeDocumentApiService
    {
        public EmployeeDocumentApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend")
        {
        }

        public async Task<ApiResponse<List<DocumentTypeModel>>> GetAllDocumentTypes()
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"api/employeeDocuments/types"));

                if (response == null)
                {
                    return new ApiResponse<List<DocumentTypeModel>>
                    {
                        Success = false,
                        Message = "Session expired"
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<DocumentTypeModel>>
                    {
                        Success = false,
                        Message = "Failed to fetch employee"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DocumentTypeModel>>>();

                return result!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse<List<DocumentCategoryModel>>> GetAllDocumentCategories()
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"api/employeeDocuments/categories"));

                if (response == null)
                {
                    return new ApiResponse<List<DocumentCategoryModel>>
                    {
                        Success = false,
                        Message = "Session expired"
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<DocumentCategoryModel>>
                    {
                        Success = false,
                        Message = "Failed to fetch employee"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DocumentCategoryModel>>>();

                return result!;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
