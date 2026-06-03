using Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool showLogin = false, bool activate = false)
        {
            ViewBag.ShowLogin = showLogin;
            ViewBag.ShowActivate = activate;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult EmployeeDashboard()
        {
            ViewBag.EmployeeName = User.Identity?.Name ?? "Employee";
            return View();
        }

    }
}
