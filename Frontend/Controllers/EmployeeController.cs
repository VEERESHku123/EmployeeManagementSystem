using AutoMapper;
using Frontend.ApiServices.Interfaces;
using Frontend.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeApiService employeeApiService;
        private readonly IDepartmentApiService departmentApi;
        private readonly IManagerApiService managerApi;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeApiService employeeApiService, IDepartmentApiService departmentApi, IManagerApiService managerApi, IMapper mapper)
        {
            this.departmentApi = departmentApi;
            this.managerApi = managerApi;
            this.mapper = mapper;
            this.employeeApiService = employeeApiService;
        }

        


        [HttpGet]
        [Route("employee/all")]
        public async Task<IActionResult> GetAllEmployees(string search, int page = 1, int pageSize = 5)
        {
            var model = await employeeApiService.GetAllEmployees(search, page, pageSize);

            // Refresh token expired
            if (!model.Success && model.Message == "Session expired")
            {
                TempData["ErrorMessage"] = "Session expired. Login again.";

                return RedirectToAction("_SignIn","User");
            }

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
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var result = await employeeApiService.GetEmployeeById(id);

            if (!result.Success && result.Message == "Session expired")
            {
                TempData["ErrorMessage"] = "Session expired";

                return RedirectToAction("_SignIn", "User");
            }

            if (result.Data == null)
            {
                return RedirectToAction("StatusCode404Page","StatusCode");
            }

            return View(result.Data);
        }


        [HttpGet]
        [Route("employee/add")]
        public async Task<IActionResult> AddNewEmployee()
        {
            var departments = await departmentApi.GetAllDepartments();
            var managers = await managerApi.SendAllManagers();

            ViewBag.Departments = new SelectList(departments.Data, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers.Data, "ManagerId", "ManagerName");

            return View();
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

            // Session expired
            if (result.Message == "Session expired")
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = result.Message ?? "Unable to create employee";

            return RedirectToAction("AddNewEmployee");
        }

        [HttpGet]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var result = await employeeApiService.GetEmployeeById(id);

            var employee = mapper.Map<UpdateEmployeeModel>(result.Data);

            var departments = await departmentApi.GetAllDepartments();
            var managers = await managerApi.SendAllManagers();

            ViewBag.Departments = new SelectList(departments.Data, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers.Data, "ManagerId", "ManagerName");

            return View(employee);
        }

        [HttpPost]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeModel model)
        {

            var result = await employeeApiService.UpdateEmployee(model.EmployeeId, model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Employee: {model.FirstName} updated successfully!";

                return RedirectToAction("GetAllEmployees");
            }

            // Session expired
            if (result.Message == "Session expired")
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = result.Message ?? "Failed to update employee";

            return RedirectToAction("UpdateEmployee",new { id = model.EmployeeId });
        }

        [HttpPost]
        [Route("employee/delete")]
        public async Task<IActionResult> DeleteEmployee([FromBody] string id)
        {
            var result = await employeeApiService.DeleteEmployee(id);

            if (result.Success)
            {
                return Ok(new
                {
                    message = "Employee deleted successfully"
                });
            }

            if (result.Message == "Session expired")
            {
                return Unauthorized(new
                {
                    message = result.Message
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
