using Backend.Data.Entitys;
using Backend.DTOs;
using Backend.DTOs.EmployeeDocument;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/employeeDocuments")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService employeeDocumentService;

        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
        {
            this.employeeDocumentService = employeeDocumentService;
        }

        [HttpGet]
        [Route("types")]
        //[Authorize]
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

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadDocuments(
    [FromForm] UploadEmployeeDocumentsModel model)
        {
            Console.WriteLine("----------------------");

            Console.WriteLine($"Files Count = {model.Files.Count}");
            Console.WriteLine($"DocumentTypeIds Count = {model.DocumentTypeIds.Count}");

            foreach (var file in model.Files)
            {
                Console.WriteLine(file.FileName);
            }

            foreach (var id in model.DocumentTypeIds)
            {
                Console.WriteLine(id);
            }

            var employeeId = User.FindFirst("employeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid token"
                });
            }

            var response = await employeeDocumentService
                .UploadEmployeeDocumentsAsync(model, employeeId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{documentId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployeeDocument(Guid documentId)
        {
            var response = await employeeDocumentService.DeleteEmployeeDocumentAsync(documentId);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpGet]
        [Route("all")]
        //[Authorize]
        public async Task<IActionResult> GetEmployeeDocuments()
        {
            var employeeId = User.FindFirst("employeeId")?.Value;

            var response = await employeeDocumentService.GetEmployeeDocuments(employeeId);

            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

    }
}
