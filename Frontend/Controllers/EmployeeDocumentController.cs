using Frontend.ApiServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class EmployeeDocumentController : Controller
    {
        private readonly IEmployeeDocumentApiService employeeDocumentApiService;

        public EmployeeDocumentController(IEmployeeDocumentApiService employeeDocumentApiService)
        {
            this.employeeDocumentApiService = employeeDocumentApiService;
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDocuments()
        {
            var documentCategoryResponse = await employeeDocumentApiService.GetAllDocumentCategories();
            var documentTypeResponse = await employeeDocumentApiService.GetAllDocumentTypes();

            ViewBag.DocumentCategories = documentCategoryResponse.Data;
            ViewBag.DocumentTypes = documentTypeResponse.Data;

            return View();
        }
    }
}
