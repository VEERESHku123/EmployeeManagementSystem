using Backend.DTOs.Common;
using Backend.DTOs.EmployeeDocument;
using Backend.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/employeeDocuments")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService employeeDocumentService;
        private readonly IBlobService blobService;

        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService, IBlobService blobService)
        {
            this.employeeDocumentService = employeeDocumentService;
            this.blobService = blobService;
        }

        [HttpGet]
        [Route("types")]
        [Authorize]
        public async Task<IActionResult> GetAllDocumentTypes()
        {
            var result = await employeeDocumentService.GetAllDocumentTypes();
            return Ok(result);
        }

        [HttpGet]
        [Route("categories")]
        [Authorize]
        public async Task<IActionResult> GetAllDocumentCategories()
        {
            var result = await employeeDocumentService.GetAllDocumentCategories();
            return Ok(result);
        }

 
        //blob
        [HttpPost("generate-upload-sas")]
        [Authorize]
        public IActionResult GenerateUploadSas(GenerateUploadSasRequest request)
        {
            var employeeId = User.FindFirst("employeeId")?.Value;

            var response = blobService.GenerateUploadSas(request.FileName, request.DocumentTypeId, employeeId);

            return Ok(
                new ApiResponse<UploadSasResponse>
                {
                    Success = true,
                    Data = response
                });
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> SaveDocument(SaveDocumentRequest request)
        {
            var employeeId = User.FindFirst("employeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Invalid token",
                    Data = false
                });
            }

            var result = await employeeDocumentService.SaveDocument(employeeId, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("my-documents/{employeeId?}")]
        [Authorize]
        public async Task<IActionResult> GetMyDocuments(string? employeeId)
        {
            if(employeeId == null) employeeId = User.FindFirst("employeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized();
            }

            var response = await employeeDocumentService.GetEmployeeDocumentsAsync(employeeId);

            return Ok(response);
        }

        [HttpDelete("delete/{employeeId}/{documentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDocument(string employeeId, Guid documentId)
        {
            var result = await employeeDocumentService.DeleteDocumentAsync(employeeId, documentId);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update/{documentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateDocument(Guid documentId, UpdateDocumentRequest request)
        {
            var employeeId = User.IsInRole("Admin")
                ? request.EmployeeId
                : User.FindFirst("employeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Invalid employee"
                });
            }

            var result = await employeeDocumentService
                .UpdateDocumentAsync(
                    employeeId,
                    documentId,
                    request);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("pending-actions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingActionDocuments()
        {
            var documents = await employeeDocumentService.GetPendingActionDocumentsAsync();
            return Ok(documents);
        }

        [HttpPut("approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveDocument(string employeeId,Guid documentId,string? remarks)
        {
            var response = await employeeDocumentService.ApproveDocumentAsync(employeeId,documentId,remarks);

            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

        [HttpPut("reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectDocument(string employeeId,Guid documentId,string remarks)
        {
            var response = await employeeDocumentService.RejectDocumentAsync(employeeId,documentId,remarks);

            return response.Success
                ? Ok(response)
                : BadRequest(response);
        }

    }
}
