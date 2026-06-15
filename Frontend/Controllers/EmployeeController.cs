using AutoMapper;
using Frontend.ApiServices.Abstracts;
using Frontend.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeApiService employeeApiService;
        private readonly IDepartmentApiService departmentApi;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeApiService employeeApiService, IDepartmentApiService departmentApi, IMapper mapper)
        {
            this.departmentApi = departmentApi;
            this.mapper = mapper;
            this.employeeApiService = employeeApiService;
        }

        


        [HttpGet]
        [Route("employee/all")]
        public async Task<IActionResult> GetAllEmployees(string search, int page = 1, int pageSize = 5)
        {
            var model = await employeeApiService.GetAllEmployees(search, page, pageSize);

            if (!model.Success)
            {
                return RedirectToAction("StatusCode500Page","StatusCode");
            }

            ViewBag.Search = search;
            ViewBag.PageSize = model.Data?.PageSize;
            ViewBag.CurrentPage = model.Data?.CurrentPage;
            ViewBag.TotalPages = model.Data?.TotalPages;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("EmployeeTable", model.Data?.Employees);
            }

            return View(model.Data?.Employees);
        }

        [HttpGet]
        [Route("employee")]
        public async Task<IActionResult> GetEmployeeById(string? employeeId)
        {
            var result = await employeeApiService.GetEmployeeById(employeeId);

            if (result.Data == null)
            {
                return RedirectToAction("StatusCode404Page","StatusCode");
            }

            return PartialView("GetEmployeeById", result.Data);
        }


        [HttpGet]
        [Route("employee/add")]
        public async Task<IActionResult> AddNewEmployee()
        {
            var departments = await departmentApi.GetAllDepartments();
            var managers = await employeeApiService.SendAllManagers();

            var designations = await employeeApiService.GetAllDesignations();

            ViewBag.Departments = new SelectList(departments.Data, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers.Data, "ManagerId", "ManagerName");

            ViewBag.Designations = new SelectList(designations.Data,"DesignationId","DesignationName");

            return PartialView("AddNewEmployee");
        }

        [HttpPost]
        [Route("employee/upload-employees")]
        public async Task<IActionResult> UploadEmployees(IFormFile file)
        {
            var response = await employeeApiService.UploadEmployeesAsync(file);

            return Json(response);
        }

        [HttpGet]
        [Route("employee/download-invalid-file")]
        public async Task<IActionResult> DownloadInvalidFile(string fileName)
        {
            var fileBytes =await employeeApiService.DownloadInvalidFileAsync(fileName);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        [Route("employee/download-template")]
        public async Task<IActionResult> DownloadTemplate()
        {
            var fileBytes = await employeeApiService.DownloadTemplateAsync();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EmployeeTemplate.xlsx");
        }

        [HttpPost]
        [Route("employee/add")]
        public async Task<IActionResult> AddNewEmployee(EmployeeModel model)
        {

            var result = await employeeApiService.AddNewEmployee(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Employee: {model.FirstName} added successfully!";

                return RedirectToAction("GetAllEmployees");
            }

            TempData["ErrorMessage"] = result.Message ?? "Unable to create employee";

            return PartialView("AddNewEmployee");
        }

        [HttpGet]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(string? employeeId)
        {
            var result = await employeeApiService.GetEmployeeById(employeeId);

            var employee = mapper.Map<UpdateEmployeeModel>(result.Data);

            var departments = await departmentApi.GetAllDepartments();

            var managers = await employeeApiService.SendAllManagers();

            var designations = await employeeApiService.GetAllDesignations();

            ViewBag.Departments = new SelectList(departments.Data, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers.Data, "ManagerId", "ManagerName");

            ViewBag.Designations = new SelectList(designations.Data, "DesignationId", "DesignationName");


            return PartialView("UpdateEmployee", employee);
        }

        [HttpPost]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeModel model)
        {

            var result = await employeeApiService.UpdateEmployee(model.EmployeeId, model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Employee: {model.FirstName} updated successfully!";

                if (User.IsInRole("Admin"))
                    return RedirectToAction("GetAllEmployees");
                else
                    return RedirectToAction("EmployeeDashboard", "Home");
            }

            // Session expired
            if (result.Message == "Session expired")
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = result.Message ?? "Failed to update employee";

            return PartialView("UpdateEmployee",new { id = model.EmployeeId });
        }

        [HttpPost]
        [Route("employee/delete")]
        public async Task<IActionResult> DeleteEmployee([FromBody] string employeeId)
        {
            var result = await employeeApiService.DeleteEmployee(employeeId);

            if (result.Success)
            {
                return Ok(new
                {
                    message = "Employee deleted successfully"
                });
            }

            return BadRequest(new
            {
                message = result.Message
            });
        }

        // validations
        [HttpGet]
        public async Task<JsonResult> IsEmailAvailable(string email)
        {
            return Json(!await employeeApiService.CheckEmailExists(email));
 
        }

        [HttpGet]
        public async Task<JsonResult> IsEmployeeIdAvailable(string employeeId)
        {
            return Json( !await employeeApiService.CheckEmployeeIdExists(employeeId));

            
        }

        [HttpGet]
        public async Task<JsonResult> IsPhoneAvailable(string phoneNumber, string? employeeId)
        {
            return Json( !await employeeApiService.CheckPhoneExists(phoneNumber, employeeId));
        }
    }
}
