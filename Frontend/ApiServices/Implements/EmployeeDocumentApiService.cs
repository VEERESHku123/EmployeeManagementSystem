using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Frontend.Models.Common;
using Frontend.Models.EmployeeDocument;
using System.Text.Json;

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

        // blob 
        public async Task<ApiResponse<UploadSasResponse>> GenerateUploadSasAsync(GenerateUploadSasRequest model)
        {
            var content = JsonContent.Create(model);

            var response =
                await SendAuthorizedRequestAsync(
                    () => client.PostAsync(
                        "api/employeeDocuments/generate-upload-sas",
                        content));

            return await response.Content.ReadFromJsonAsync<ApiResponse<UploadSasResponse>>();
        }

        public async Task<ApiResponse<bool>> UploadDocumentsAsync(int documentTypeId, IFormFile file)
        {
            // STEP 1: Generate SAS

            var sasResponse =
                await GenerateUploadSasAsync(
                    new GenerateUploadSasRequest
                    {
                        DocumentTypeId = documentTypeId,
                        FileName = file.FileName
                    });

            if (!sasResponse.Success)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = sasResponse.Message
                };
            }

            // STEP 2: Upload to Blob

            using var blobClient = new HttpClient();

            using var stream = file.OpenReadStream();

            using var content = new StreamContent(stream);

            content.Headers.Add("x-ms-blob-type", "BlockBlob");
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            var uploadResult = await blobClient.PutAsync(sasResponse.Data.UploadUrl,content);

            if (!uploadResult.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Blob upload failed."
                };
            }

            // STEP 3: Save Metadata

            var saveModel = new SaveDocumentRequest
            {
                DocumentTypeId = documentTypeId,
                BlobName = sasResponse.Data.BlobName
            };

            var saveContent = JsonContent.Create(saveModel);

            var saveResponse =
                await SendAuthorizedRequestAsync(
                    () => client.PostAsync(
                        "api/employeeDocuments/save",
                        saveContent));

            var responseText =
                await saveResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {saveResponse.StatusCode}");
            Console.WriteLine($"Response: {responseText}");

            if (!saveResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"API Error: {saveResponse.StatusCode}"
                };
            }

            var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<bool>>(
    responseText,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

            Console.WriteLine($"Success: {apiResponse?.Success}");
            Console.WriteLine($"Message: {apiResponse?.Message}");
            Console.WriteLine($"Data: {apiResponse?.Data}");

            return apiResponse!;
        }

        public async Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocumentsAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"api/employeeDocuments/my-documents"));

            if (response == null)
            {
                return new ApiResponse<List<EmployeeDocumentModel>>
                {
                    Success = false,
                    Message = "Session expired. Please login again."
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                return new ApiResponse<List<EmployeeDocumentModel>>
                {
                    Success = false,
                    Message = error
                };
            }

            var result =  await response.Content.ReadFromJsonAsync<ApiResponse<List<EmployeeDocumentModel>>>();

            if (result != null)
            {
                Console.WriteLine($"Success: {result.Success}");
                Console.WriteLine($"Message: {result.Message}");
                Console.WriteLine($"Count: {result.Data?.Count}");
            }
            return result;
        }
    }

}
