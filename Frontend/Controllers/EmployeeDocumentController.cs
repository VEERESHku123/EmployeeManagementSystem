using Frontend.ApiServices.Interfaces;
using Frontend.Models.EmployeeDocument;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

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
            if (model.File == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Please select a file."
                });
            }

            var response =
                await employeeDocumentApiService.UploadDocumentsAsync(
                    model.DocumentTypeId,
                    model.File);

            if (response == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "No response received."
                });
            }

            return Json(response);
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

        [HttpGet]
        public async Task<IActionResult> MyDocuments()
        {
            var response = await employeeDocumentApiService.GetEmployeeDocumentsAsync();

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;

                return View(
                    new List<EmployeeDocumentModel>());
            }

            return View(response.Data);
        }
    }
}
