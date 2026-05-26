using Backend.Data.Entitys;
using Backend.DTOs;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/employeeDocuments")]
    [ApiController]
    [Authorize]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService employeeDocumentService;

        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
        {
            this.employeeDocumentService = employeeDocumentService;
        }

        [HttpGet]
        [Route("types")]
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
    }
}
