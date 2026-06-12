using Frontend.ApiServices.Abstracts;
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

        #region Employee

        [HttpGet("document/upload")]
        public async Task<IActionResult> UploadDocument(string? employeeId)
        {
            var categoryResponse = await employeeDocumentApiService.GetAllDocumentCategories();

            var typeResponse = await employeeDocumentApiService.GetAllDocumentTypes();

            var uploadedResponse = await employeeDocumentApiService.GetEmployeeDocumentsAsync(employeeId);

            ViewBag.DocumentCategories = categoryResponse.Data;
            ViewBag.DocumentTypes = typeResponse.Data;
            ViewBag.UploadedDocuments = uploadedResponse.Data;

            return PartialView("UploadDocument");
        }

        [HttpPost("document/upload")]
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

        [HttpPost("document/update")]
        public async Task<IActionResult> UpdateDocument(string employeeId, Guid documentId, int documentTypeId, IFormFile file)
        {
            var result = await employeeDocumentApiService.UpdateDocumentAsync(
                documentId,
                documentTypeId,
                file,
                employeeId);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        #endregion

        #region Admin

        [HttpGet("document/PendingDocuments")]
        public async Task<IActionResult> PendingDocuments()
        {
            var response = await employeeDocumentApiService.GetPendingDocumentsAsync();

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;

                return View(new List<PendingDocumentModel>());
            }

            return PartialView("PendingDocuments", response.Data);
        }

        [HttpGet("document/verify")]
        public async Task<IActionResult> VerifyDocuments(string employeeId)
        {
            var categoryResponse = await employeeDocumentApiService.GetAllDocumentCategories();

            var typeResponse = await employeeDocumentApiService.GetAllDocumentTypes();

            var uploadedResponse = await employeeDocumentApiService.GetEmployeeDocumentsAsync(employeeId);

            ViewBag.EmployeeId = employeeId;
            ViewBag.DocumentCategories = categoryResponse.Data;
            ViewBag.DocumentTypes = typeResponse.Data;
            ViewBag.UploadedDocuments = uploadedResponse.Data;

            return PartialView("VerifyDocuments");
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

        [HttpPost]
        public async Task<IActionResult> ApproveDocument(string employeeId, Guid documentId, string? remarks)
        {
            var result = await employeeDocumentApiService.ApproveDocumentAsync(employeeId, documentId, remarks);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> RejectDocument(string employeeId,Guid documentId,string remarks)
        {
            var result = await employeeDocumentApiService.RejectDocumentAsync(employeeId, documentId, remarks);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        #endregion
    }
}