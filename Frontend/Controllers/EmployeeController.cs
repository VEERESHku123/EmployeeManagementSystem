using AutoMapper;
using Frontend.ApiServices.Implements;
using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices empService;
        private readonly IDepartmentApiService departmentApi;
        private readonly IManagerApiService managerApi;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeServices empService, IDepartmentApiService departmentApi, IManagerApiService managerApi, IMapper mapper)
        {
            this.departmentApi = departmentApi;
            this.managerApi = managerApi;
            this.mapper = mapper;
            this.empService = empService;
        }

        


        [HttpGet]
        [Route("employee/all")]
        public async Task<IActionResult> GetAllEmployees(string search, int page = 1, int pageSize = 5)
        {
            var model = await empService.GetAllEmployees(search, page, pageSize);

            if (model.StatusCode == 500)
            {
                return RedirectToAction("StatusCode500Page","StatusCode");
            }

            ViewBag.Search = model.Search;
            ViewBag.PageSize = model.PageSize;
            ViewBag.CurrentPage = model.CurrentPage;
            ViewBag.TotalPages = model.TotalPages;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("EmployeeTable", model.Employees);
            }

            return View(model.Employees);
        }

        [HttpGet]
        [Route("employee")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var result = await empService.GetEmployeeById(id);
            if (result.StatusCode == 500)
            {
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }
            else if(result.StatusCode == 404)
            {
                return RedirectToAction("StatusCode404Page", "StatusCode");
            }

            return View(result.Employee);
        }


        [HttpGet]
        [Route("employee/add")]
        public async Task<IActionResult> AddNewEmployee()
        {
            var departments = await departmentApi.GetAllDepartments();
            var managers = await managerApi.SendAllManagers();

            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers, "ManagerId", "ManagerName");

            return View();
        }

        [HttpPost]
        [Route("employee/add")]
        public async Task<IActionResult> AddNewEmployee(EmployeeModel model)
        {

            var isSuccess = await empService.AddNewEmployee(model);

            if(isSuccess)
            {
                TempData["SuccessMessage"] = $"Employee: {model.FirstName} added successfully!";
                return RedirectToAction("GetAllEmployees");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add employee!";
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }
        }

        [HttpGet]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var result = await empService.GetEmployeeById(id);

            if(result.StatusCode == 404)
            {
                return RedirectToAction("StatusCode404Page", "StatusCode");
            }
            var employee = mapper.Map<UpdateEmployeeModel>(result.Employee);

            var departments = await departmentApi.GetAllDepartments();
            var managers = await managerApi.SendAllManagers();

            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers, "ManagerId", "ManagerName");

            return View(employee);
        }

        [HttpPost]
        [Route("employee/update")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeModel model)
        {

            var result = await empService.UpdateEmployee(model.EmployeeId, model);


            if (result == 200)
            {
                TempData["SuccessMessage"] = $"Employee: {model.FirstName} updated successfully!";
                return RedirectToAction("GetAllEmployees");
            }
            else if(result == 404)
            {
                TempData["ErrorMessage"] = "Failed to update employee!";
                return RedirectToAction("StatusCode404Page", "StatusCode");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update employee!";
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }
        }

        [HttpPost]
        [Route("employee/delete")]
        public async Task<IActionResult> DeleteEmployee([FromBody] string id)
        {
            var result = await empService.DeleteEmployee(id);

            if (result == 200)
                return Ok();

            return BadRequest();
        }

        // validations
        [HttpGet]
        public async Task<JsonResult> IsEmailAvailable(string email)
        {
            return Json(await empService.IsEmailAvailable(email));
 
        }

        [HttpGet]
        public async Task<JsonResult> IsEmployeeIdAvailable(string employeeId)
        {
            return Json( await empService.IsEmployeeIdAvailable(employeeId));

            
        }

        [HttpGet]
        public async Task<JsonResult> IsPhoneAvailable(string phoneNumber, string? employeeId)
        {
            return Json( await empService.IsPhoneAvailable(phoneNumber, employeeId));
        }
    }
}
