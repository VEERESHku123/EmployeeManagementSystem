using Frontend.APIs;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        public EmployeeController(EmployeeAPI employeeAPI, DepartmentAPI departmentAPI, ManagerAPI managerAPI)
        {
            DepartmentAPI = departmentAPI;
            ManagerAPI = managerAPI;
            EmployeeAPI = employeeAPI;
        }

        public EmployeeAPI EmployeeAPI { get; set; }
        public DepartmentAPI DepartmentAPI { get; set; }
        public ManagerAPI ManagerAPI { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(string search, int page = 1, int pageSize = 5)
        {
            var result = await EmployeeAPI.SendAllEmployee(search, page, pageSize);

            if (result.StatusCode == 500)
            {
                return RedirectToAction("StatusCode500Page", "StatusCode");
            }

            ViewBag.Search = search;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

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
            var isSuccess = await EmployeeAPI.AddNewEmployee(model);

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

            return View(result.Employee);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(EmployeeModel model)
        {
            var result = await EmployeeAPI.UpdateEmployee(model.EmployeeId, model);
            Console.WriteLine(model.EmployeeId);

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

        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await EmployeeAPI.DeleteEmployee(id);
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
    }
}
