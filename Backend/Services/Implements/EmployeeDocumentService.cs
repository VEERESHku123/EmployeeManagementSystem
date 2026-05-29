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
        
        public async Task<ApiResponse<bool>> SaveDocument(string employeeId, SaveDocumentRequest request)
        {
            var entity =
                new EmployeeDocumentEntity
                {
                    EmployeeId = employeeId!,
                    DocumentTypeId = request.DocumentTypeId,
                    BlobName = request.BlobName,
                    UploadedDate = DateTime.Now,
                    VerificationStatus = "Pending"
                };

            var result = await employeeDocumentRepo.SaveDocumentAsync(entity);

            return new ApiResponse<bool>
            {
                Success = result,
                Message = result
                        ? "Document saved successfully."
                        : "Failed to save document.",
                Data = result
            };
        }

        public async Task<ApiResponse< List<EmployeeDocumentDto>>> GetEmployeeDocumentsAsync(string employeeId)
        {
            var documents = await employeeDocumentRepo.GetEmployeeDocumentsAsync(employeeId);

            if (documents == null || !documents.Any())
            {
                return new ApiResponse<List<EmployeeDocumentDto>>
                {
                    Success = false,
                    Message = "No documents found.",
                    Data = new List<EmployeeDocumentDto>()
                };
            }

            var result = mapper.Map<List<EmployeeDocumentDto>>(documents);

            foreach (var document in result)
            {
                document.DownloadUrl =
                    blobService.GenerateReadSas(document.BlobName);
            }

            return new ApiResponse<List<EmployeeDocumentDto>>
            {
                Success = true,
                Message = "Documents retrieved successfully.",
                Data = result
            };
        }
    }
}
