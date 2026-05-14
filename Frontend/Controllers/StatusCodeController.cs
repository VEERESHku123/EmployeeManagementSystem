using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class StatusCodeController : Controller
    {
        public IActionResult StatusCode500Page()
        {
            return View();
        }

        public IActionResult StatusCode404Page()
        {
            return View();
        }
    }
}
