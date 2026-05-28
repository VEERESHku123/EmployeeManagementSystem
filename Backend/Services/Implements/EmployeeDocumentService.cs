using AutoMapper;
using Backend.Data.Entitys;
using Backend.Data.Repos.Interfaces;
using Backend.DTOs;
using Backend.DTOs.EmployeeDocument;
using Backend.Services.Interfaces;

namespace Backend.Services.Implements
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepo employeeDocumentRepo;
        private readonly IBlobService blobService;
        private readonly IMapper mapper;

        public EmployeeDocumentService(IEmployeeDocumentRepo employeeDocumentRepo, IMapper mapper, IBlobService blobService)
        {
            this.employeeDocumentRepo = employeeDocumentRepo;
            this.blobService = blobService;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<DocumentTypeEntity>>> GetAllDocumentTypes()
        {
            var data = await employeeDocumentRepo.GetAllDocumentTypesAsync();

            return new ApiResponse<List<DocumentTypeEntity>>
            {
                Success = true,
                Message = "Fetched successfully",
                Data = data
            };
        }

        public async Task<ApiResponse<List<DocumentCategoryEntity>>> GetAllDocumentCategories()
        {
            var data = await employeeDocumentRepo.GetAllDocumentCategoriesAsync();

            return new ApiResponse<List<DocumentCategoryEntity>>
            {
                Success = true,
                Message = "Fetched successfully",
                Data = data
            };
        }

        public async Task<ApiResponse<string>> UploadEmployeeDocumentsAsync(UploadEmployeeDocumentsModel model, string employeeId)
        {
            if (model.Files == null || model.Files.Count == 0)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "No files uploaded",
                    Data = null
                };
            }

            if (model.DocumentTypeIds == null || model.DocumentTypeIds.Count != model.Files.Count)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Document types count must match files count",
                    Data = null
                }; 
            }

            List<EmployeeDocumentEntity> documents = new List<EmployeeDocumentEntity>();

            for (int i = 0; i < model.Files.Count; i++)
            {
                var file = model.Files[i];

                // Upload to Blob Storage
                FileUploadResponseDto uploadedFile = await blobService.UploadFileAsync(file);

                // Save metadata
                EmployeeDocumentEntity document = new EmployeeDocumentEntity
                {
                    EmployeeId =employeeId,
                    DocumentTypeId = model.DocumentTypeIds[i],

                    FileName = uploadedFile.FileName,
                    FileUrl = uploadedFile.FileUrl,

                    UploadedDate = DateTime.Now
                };

                documents.Add(document);
            }
            
            await employeeDocumentRepo.SaveDocumentsAsync(documents);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Documents uploaded successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteEmployeeDocumentAsync(Guid documentId)
        {
            var result = await employeeDocumentRepo.DeleteDocumentAsync(documentId);

            if(result)
            {
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Document deleted successfully",
                    Data = null
                };
            }
            else
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Document not found",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<EmployeeDocumentDto>>> GetEmployeeDocuments(string employeeId)
        {
            var result = await employeeDocumentRepo
                .GetEmployeeDocuments(employeeId);

            if (result == null || !result.Any())
            {
                return new ApiResponse<List<EmployeeDocumentDto>>
                {
                    Success = false,
                    Message = "No documents found",
                    Data = null
                };
            }

            var documents = mapper.Map<List<EmployeeDocumentDto>>(result);

            return new ApiResponse<List<EmployeeDocumentDto>>
            {
                Success = true,
                Message = "Documents fetched successfully",
                Data = documents
            };
        }
    }
}
