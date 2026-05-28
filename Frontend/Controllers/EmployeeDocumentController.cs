using Frontend.ApiServices.Interfaces;
using Frontend.Models.EmployeeDocument;
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
        public async Task<IActionResult> UploadDocuments()
        {
            var documentCategoryResponse = await employeeDocumentApiService.GetAllDocumentCategories();
            var documentTypeResponse = await employeeDocumentApiService.GetAllDocumentTypes();

            ViewBag.DocumentCategories = documentCategoryResponse.Data;
            ViewBag.DocumentTypes = documentTypeResponse.Data;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocuments(UploadEmployeeDocumentsModel model)
        {
            Console.WriteLine("========== FRONTEND CONTROLLER ==========");

            Console.WriteLine($"DocumentTypeIds Count = {model.DocumentTypeIds.Count}");
            Console.WriteLine($"Files Count = {model.Files.Count}");

            foreach (var id in model.DocumentTypeIds)
            {
                Console.WriteLine($"DocTypeId = {id}");
            }

            foreach (var file in model.Files)
            {
                Console.WriteLine($"File = {file?.FileName}");
            }

            var response = await employeeDocumentApiService.UploadDocumentsAsync(model);

            if (response.Success)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return RedirectToAction("ViewDocuments");
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocuments()
        {
            var result = await employeeDocumentApiService.GetEmployeeDocuments();

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return View(new List<EmployeeDocumentModel>());
            }

            return View(result.Data);
        }
    }
}
