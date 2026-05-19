using AutoMapper;
using Frontend.APIs;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        public EmployeeController(EmployeeAPI employeeAPI, DepartmentAPI departmentAPI, ManagerAPI managerAPI, IMapper mapper)
        {
            DepartmentAPI = departmentAPI;
            ManagerAPI = managerAPI;
            EmployeeAPI = employeeAPI;
            Mapper = mapper;
        }

        public EmployeeAPI EmployeeAPI { get; set; }
        public DepartmentAPI DepartmentAPI { get; set; }
        public ManagerAPI ManagerAPI { get; set; }
        public IMapper Mapper { get; set; }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(string search, int page = 1, int pageSize = 5)
        {
            if (search != null) search.Trim();

            var result = await EmployeeAPI.SendAllEmployee(search, page, pageSize);

            if (result.StatusCode == 500)
            {
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }

            ViewBag.Search = search;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            // AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("EmployeeTable", result.Employees);
            }

            // Normal page load
            return View(result.Employees);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var result = await EmployeeAPI.SendEmployeeById(id);
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
        public async Task<IActionResult> AddNewEmployee()
        {
            var departmentsResponse = await DepartmentAPI.SendAllDepartments();
            var managersResponse = await ManagerAPI.SendAllManagers();

            var departments = departmentsResponse.departmentList;
            var managers = managersResponse.managersList;

            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            ViewBag.Managers = new SelectList(managers, "ManagerId", "ManagerName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(EmployeeModel model)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            var isSuccess = await EmployeeAPI.AddNewEmployee(model, token);

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
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var result = await EmployeeAPI.SendEmployeeById(id);

            if(result.StatusCode == 404)
            {
                return RedirectToAction("StatusCode404Page", "StatusCode");
            }
            var employee = Mapper.Map<UpdateEmployeeModel>(result.Employee);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeModel model)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            var result = await EmployeeAPI.UpdateEmployee(model.EmployeeId, model, token);


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

        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            Console.WriteLine("id: " + id);
            var result = await EmployeeAPI.DeleteEmployee(id, token);
            if (result == 200)
            {
                TempData["SuccessMessage"] = $"Employee Id: {id} deleted successfully!";
                return RedirectToAction("GetAllEmployees");
            }
            else if(result == 404)
            {
                TempData["ErrorMessage"] = "Failed to delete employee!";
                return RedirectToAction("StatusCode404Page", "StatusCode");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete employee!";
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }
        }

        // validations
        [HttpGet]
        public async Task<JsonResult> IsEmailAvailable(string email)
        {
            var exists = await EmployeeAPI.CheckEmailExists(email);

            return Json(!exists); 
        }

        [HttpGet]
        public async Task<JsonResult> IsEmployeeIdAvailable(string employeeId)
        {
            var exists = await EmployeeAPI.CheckEmployeeIdExists(employeeId);

            return Json(!exists);
        }

        [HttpGet]
        public async Task<JsonResult> IsPhoneAvailable(string phoneNumber, string? employeeId)
        {
            var exists = await EmployeeAPI.CheckPhoneExists(phoneNumber, employeeId);

            return Json(!exists);
        }
    }
}
