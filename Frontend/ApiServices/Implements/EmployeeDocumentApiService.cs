using Frontend.ApiServices.Interfaces;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using Frontend.Models.EmployeeDocument;
using Frontend.Views.EmployeeDocument;
using Newtonsoft.Json;

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

        public async Task<ApiResponse<string>> UploadDocumentsAsync(
     UploadEmployeeDocumentsModel model)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                Console.WriteLine("========== API SERVICE ==========");

                // DOCUMENT TYPE IDS
                foreach (var id in model.DocumentTypeIds)
                {
                    Console.WriteLine($"Sending DocTypeId = {id}");

                    content.Add(
                        new StringContent(id.ToString()),
                        "DocumentTypeIds"
                    );
                }

                // FILES
                foreach (var file in model.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        Console.WriteLine($"Sending File = {file.FileName}");

                        var streamContent = new StreamContent(file.OpenReadStream());

                        streamContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                        content.Add(
                            streamContent,
                            "Files",
                            file.FileName
                        );
                    }
                }

                var response = await SendAuthorizedRequestAsync(() =>
                    client.PostAsync("api/employeeDocuments/upload", content));

                if (response == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Session expired"
                    };
                }

                Console.WriteLine($"Status = {response.StatusCode}");

                var result = await response.Content
                    .ReadFromJsonAsync<ApiResponse<string>>();

                return result ?? new ApiResponse<string>
                {
                    Success = false,
                    Message = "Something went wrong"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocuments()
        {
            try
            {

                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"api/employeeDocuments/all"));
                if (response == null)
                {
                    return new ApiResponse<List<EmployeeDocumentModel>>
                    {
                        Success = false,
                        Message = "Session expired"
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<EmployeeDocumentModel>>
                    {
                        Success = false,
                        Message = "Failed to upload documents"
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<EmployeeDocumentModel>>>();

                return result ?? new ApiResponse<List<EmployeeDocumentModel>>
                {
                    Success = false,
                    Message = "Something went wrong"
                };
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
