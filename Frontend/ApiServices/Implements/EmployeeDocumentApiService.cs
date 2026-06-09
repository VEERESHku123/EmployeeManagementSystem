using Frontend.ApiServices.Abstracts;
using Frontend.Models;
using Frontend.Models.Common;
using Frontend.Models.EmployeeDocument;

namespace Frontend.ApiServices.Implements
{
    public class EmployeeDocumentApiService : BaseApiService, IEmployeeDocumentApiService
    {
        public EmployeeDocumentApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : 
            base(factory, httpContextAccessor, "Backend") { }
  

        public async Task<ApiResponse<List<DocumentTypeModel>>> GetAllDocumentTypes()
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"employeeDocuments/types"));

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
                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"employeeDocuments/categories"));

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

                var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"employeeDocuments/all"));
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

            var response = await SendAuthorizedRequestAsync(() => client.PostAsync("employeeDocuments/generate-upload-sas",content));

            return await response.Content.ReadFromJsonAsync<ApiResponse<UploadSasResponse>>();
        }

        public async Task<ApiResponse<bool>> UploadDocumentsAsync(int documentTypeId, IFormFile file)
        {
            // STEP 1: Generate SAS

            var sasResponse = await GenerateUploadSasAsync(
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

            var saveResponse = await SendAuthorizedRequestAsync(() => client.PostAsJsonAsync("employeeDocuments/save", saveModel));

            if (!saveResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"API Error: {saveResponse.StatusCode}"
                };
            }

            return await saveResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }

        public async Task<ApiResponse<List<EmployeeDocumentModel>>> GetEmployeeDocumentsAsync(string? employeeId)
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"employeeDocuments/my-documents/{employeeId}"));

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
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse<List<EmployeeDocumentModel>>
                {
                    Success = false,
                    Message = error
                };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<List<EmployeeDocumentModel>>>();

        }

        public async Task<ApiResponse<bool>> DeleteDocumentAsync(string employeeId, Guid documentId)
        {
            var response = await SendAuthorizedRequestAsync(() => client.DeleteAsync($"employeeDocuments/delete/{employeeId}/{documentId}"));

            if (response == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Session expired. Please login again."
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = error
                };
            }

            return await response.Content
                .ReadFromJsonAsync<ApiResponse<bool>>()
                ?? new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Failed to delete document"
                };
        }

        public async Task<ApiResponse<bool>> UpdateDocumentAsync(Guid documentId,int documentTypeId,IFormFile file,string? employeeId = null)
        {
            // Generate SAS

            var sasResponse = await GenerateUploadSasAsync(
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

            // Upload file to blob

            using var blobClient = new HttpClient();

            using var stream = file.OpenReadStream();

            using var content = new StreamContent(stream);

            content.Headers.Add("x-ms-blob-type", "BlockBlob");

            content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            var uploadResult = await blobClient.PutAsync(sasResponse.Data.UploadUrl, content);

            if (!uploadResult.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Blob upload failed."
                };
            }

            // Update API

            var request = new UpdateDocumentRequest
            {
                EmployeeId = employeeId, // null for employee, value for admin
                BlobName = sasResponse.Data.BlobName
            };

            var response = await SendAuthorizedRequestAsync(() => client.PutAsJsonAsync($"employeeDocuments/update/{documentId}",request));

            if (response == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Session expired. Please login again."
                };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
                ?? new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Failed to update document."
                };
        }

        public async Task<ApiResponse<List<PendingDocumentModel>>> GetPendingDocumentsAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync("employeeDocuments/pending-actions"));

            if (response == null)
            {
                return new ApiResponse<List<PendingDocumentModel>>
                {
                    Success = false,
                    Message = "Session expired. Please login again."
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse<List<PendingDocumentModel>>
                {
                    Success = false,
                    Message = error
                };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<List<PendingDocumentModel>>>();
        }

        public async Task<ApiResponse<bool>> ApproveDocumentAsync(string employeeId,Guid documentId,string? remarks)
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.PutAsync(
                    $"employeeDocuments/approve?employeeId={employeeId}&documentId={documentId}&remarks={Uri.EscapeDataString(remarks ?? string.Empty)}", null));

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                Console.WriteLine(result);

                return result ?? new ApiResponse<bool>
                {
                    Success = false,
                    Message = "No response received."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> RejectDocumentAsync(string employeeId,Guid documentId,string remarks)
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.PutAsync(
                    $"employeeDocuments/reject?employeeId={employeeId}&documentId={documentId}&remarks={Uri.EscapeDataString(remarks)}", null));

                var result = await response.Content
                    .ReadFromJsonAsync<ApiResponse<bool>>();

                return result ?? new ApiResponse<bool>
                {
                    Success = false,
                    Message = "No response received."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
