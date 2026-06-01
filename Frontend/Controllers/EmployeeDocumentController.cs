using Frontend.ApiServices.Abstracts;
using Frontend.Models.EmployeeDocument;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class EmployeeDocumentController : Controller
    {
        private readonly IEmployeeDocumentApiService employeeDocumentApiService;

        public EmployeeDocumentController(
            IEmployeeDocumentApiService employeeDocumentApiService)
        {
            this.employeeDocumentApiService = employeeDocumentApiService;
        }

        #region Employee

        [HttpGet]
        public async Task<IActionResult> UploadDocument(string? employeeId)
        {
            var categoryResponse = await employeeDocumentApiService.GetAllDocumentCategories();

            var typeResponse = await employeeDocumentApiService.GetAllDocumentTypes();

            var uploadedResponse = await employeeDocumentApiService.GetEmployeeDocumentsAsync(employeeId);

            ViewBag.DocumentCategories = categoryResponse.Data;
            ViewBag.DocumentTypes = typeResponse.Data;
            ViewBag.UploadedDocuments = uploadedResponse.Data;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(UploadEmployeeDocumentsModel model)
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

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDocument(string employeeId,Guid documentId,int documentTypeId,IFormFile file)
        {
            var result =
                await employeeDocumentApiService.UpdateDocumentAsync(
                    documentId,
                    documentTypeId,
                    file,
                    employeeId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(UploadDocument),
                new { employeeId });
        }

        #endregion

        #region Admin

        [HttpGet]
        public async Task<IActionResult> PendingDocuments()
        {
            var response = await employeeDocumentApiService.GetPendingDocumentsAsync();

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;

                return View(new List<PendingDocumentModel>());
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDocuments(string employeeId)
        {
            var categoryResponse =
                await employeeDocumentApiService.GetAllDocumentCategories();

            var typeResponse =
                await employeeDocumentApiService.GetAllDocumentTypes();

            var uploadedResponse =
                await employeeDocumentApiService.GetEmployeeDocumentsAsync(employeeId);

            ViewBag.EmployeeId = employeeId;
            ViewBag.DocumentCategories = categoryResponse.Data;
            ViewBag.DocumentTypes = typeResponse.Data;
            ViewBag.UploadedDocuments = uploadedResponse.Data;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(string employeeId,Guid documentId)
        {
            var result = await employeeDocumentApiService.DeleteDocumentAsync(employeeId,documentId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(VerifyDocuments),new { employeeId });
        }

        #endregion
    }
}