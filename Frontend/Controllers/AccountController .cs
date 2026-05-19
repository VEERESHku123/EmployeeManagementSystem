using Frontend.APIs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountApi accountApi;

        public AccountController(AccountApi accountApi)
        {
            this.accountApi = accountApi;
        }

        public IActionResult Login()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/account/callback"
            };

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
