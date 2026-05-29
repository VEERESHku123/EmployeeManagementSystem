using Backend.DTOs;
using Backend.Services.Interfaces;
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

            var response =blobService.GenerateUploadSas(request.FileName, employeeId);

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

        [HttpGet("my-documents")]
        [Authorize]
        public async Task<IActionResult> GetMyDocuments()
        {
            var employeeId =
                User.FindFirst("employeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized();
            }

            var response = await employeeDocumentService.GetEmployeeDocumentsAsync(employeeId);

            return Ok(response);
        }
    }
}
