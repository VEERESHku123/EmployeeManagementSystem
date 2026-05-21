using Frontend.ApiServices.Implements;
using Frontend.ApiServices.Interfaces;
using Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountApiService accountApi;

        public AccountController(IAccountApiService accountApi)
        {
            this.accountApi = accountApi;
        }


        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            
            return View();
        }

        public IActionResult MicrosoftSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/account/callback"
            };

            properties.Items["prompt"] = "select_account";

            return Challenge(properties,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Callback()
        {
            var email = User.FindFirst("preferred_username")?.Value;
            var name = User.Identity?.Name;

            var auth = await accountApi.GetJwtToken(email, name);

            HttpContext.Session.SetString("JwtToken", auth.Token);
            
            HttpContext.Session.SetString("Role", auth.RoleType);

            HttpContext.Session.SetString("Email", email);

            TempData["SuccessMessage"] = "Login successful";

            return RedirectToAction("GetAllEmployees", "Employee");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "Logged out successfully";

            return SignOut(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }


}
