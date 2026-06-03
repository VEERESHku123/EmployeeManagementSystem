using AutoMapper;
using Backend.Data.Entities;
using Backend.Data.Repos.Abstracts;
using Backend.DTOs.Common;
using Backend.DTOs.EmployeeDocument;
using Backend.Services.Abstracts;

namespace Backend.Services.Implements
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepo employeeDocumentRepo;
        private readonly IBlobService blobService;
        private readonly ILogger<EmployeeDocumentService> logger;
        private readonly IMapper mapper;

        public EmployeeDocumentService(IEmployeeDocumentRepo employeeDocumentRepo, IMapper mapper, IBlobService blobService, ILogger<EmployeeDocumentService> logger)
        {
            this.employeeDocumentRepo = employeeDocumentRepo;
            this.blobService = blobService;
            this.mapper = mapper;
            this.logger = logger;
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
            try
            {
                logger.LogInformation(
                    "Saving document for EmployeeId {EmployeeId}, DocumentTypeId {DocumentTypeId}",
                    employeeId,
                    request.DocumentTypeId);

                var entity = new EmployeeDocumentEntity
                {
                    EmployeeId = employeeId,
                    DocumentTypeId = request.DocumentTypeId,
                    BlobName = request.BlobName,
                    UploadedDate = DateTime.UtcNow,
                    VerificationStatus = "Pending"
                };

                var result = await employeeDocumentRepo.SaveDocumentAsync(entity);

                logger.LogInformation(
                    "Document save operation completed for EmployeeId {EmployeeId}. Result: {Result}",
                    employeeId,
                    result);

                return new ApiResponse<bool>
                {
                    Success = result,
                    Message = result
                        ? "Document saved successfully."
                        : "Failed to save document.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while saving document for EmployeeId {EmployeeId}",
                    employeeId);

                throw;
            }
        }

        public async Task<ApiResponse<List<EmployeeDocumentDto>>> GetEmployeeDocumentsAsync(string employeeId)
        {
            try
            {
                logger.LogInformation(
                    "Retrieving documents for EmployeeId {EmployeeId}",
                    employeeId);

                var documents = await employeeDocumentRepo.GetEmployeeDocumentsAsync(employeeId);

                if (documents == null || !documents.Any())
                {
                    logger.LogWarning(
                        "No documents found for EmployeeId {EmployeeId}",
                        employeeId);

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

                logger.LogInformation(
                    "Retrieved {DocumentCount} documents for EmployeeId {EmployeeId}",
                    result.Count,
                    employeeId);

                return new ApiResponse<List<EmployeeDocumentDto>>
                {
                    Success = true,
                    Message = "Documents retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while retrieving documents for EmployeeId {EmployeeId}",
                    employeeId);

                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteDocumentAsync(string employeeId,Guid documentId)
        {
            try
            {
                logger.LogInformation(
                    "Deleting document {DocumentId} for employee {EmployeeId}",
                    documentId,
                    employeeId);

                var document = await employeeDocumentRepo
                    .GetDocumentAsync(employeeId, documentId);

                if (document == null)
                {
                    logger.LogWarning(
                        "Document {DocumentId} not found for employee {EmployeeId}",
                        documentId,
                        employeeId);

                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Document not found",
                        Data = false
                    };
                }

                await blobService.DeleteBlobAsync(document.BlobName);

                await employeeDocumentRepo.DeleteAsync(document);

                logger.LogInformation(
                    "Document {DocumentId} deleted successfully",
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Document deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error deleting document {DocumentId} for employee {EmployeeId}",
                    documentId,
                    employeeId);

                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateDocumentAsync(string employeeId, Guid documentId, UpdateDocumentRequest request)
        {
            try
            {
                logger.LogInformation(
                    "Updating document {DocumentId} for employee {EmployeeId}",
                    documentId,
                    employeeId);

                var document = await employeeDocumentRepo
                    .GetDocumentAsync(employeeId, documentId);

                if (document == null)
                {
                    logger.LogWarning(
                        "Document {DocumentId} not found for employee {EmployeeId}",
                        documentId,
                        employeeId);

                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Document not found",
                        Data = false
                    };
                }

                await blobService.DeleteBlobAsync(document.BlobName);

                await employeeDocumentRepo.UpdateDocumentAsync(
                    document,
                    request.BlobName);

                logger.LogInformation(
                    "Document {DocumentId} updated successfully",
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Document updated successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error updating document {DocumentId} for employee {EmployeeId}",
                    documentId,
                    employeeId);

                throw;
            }
        }

        public async Task<ApiResponse<List<PendingDocumentDto>>> GetPendingActionDocumentsAsync()
        {
            var documents = await employeeDocumentRepo.GetPendingActionDocumentsAsync();

            return new ApiResponse<List<PendingDocumentDto>>
            {
                Success = true,
                Message = "Pending action documents fetched successfully.",
                Data = documents
            };
        }

        public async Task<ApiResponse<bool>> ApproveDocumentAsync(string employeeId,Guid documentId,string? remarks)
        {
            try
            {
                logger.LogInformation(
                    "Approving document. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                var result =
                    await employeeDocumentRepo
                        .ApproveDocumentAsync(
                            employeeId,
                            documentId,
                            remarks);

                if (!result)
                {
                    logger.LogWarning(
                        "Document not found. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                        employeeId,
                        documentId);

                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Document not found."
                    };
                }

                logger.LogInformation(
                    "Document approved successfully. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Document approved successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error approving document. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while approving the document."
                };
            }
        }

        public async Task<ApiResponse<bool>> RejectDocumentAsync(string employeeId,Guid documentId,string remarks)
        {
            try
            {
                logger.LogInformation(
                    "Rejecting document. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                var result =
                    await employeeDocumentRepo
                        .RejectDocumentAsync(
                            employeeId,
                            documentId,
                            remarks);

                if (!result)
                {
                    logger.LogWarning(
                        "Document not found. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                        employeeId,
                        documentId);

                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Document not found."
                    };
                }

                logger.LogInformation(
                    "Document rejected successfully. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Document rejected successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error rejecting document. EmployeeId: {EmployeeId}, DocumentId: {DocumentId}",
                    employeeId,
                    documentId);

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while rejecting the document."
                };
            }
        }
    }
}
