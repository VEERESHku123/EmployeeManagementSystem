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

            properties.Items["prompt"] = "login";

            return Challenge(properties,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Callback()
        {
            var email = User.FindFirst("preferred_username")?.Value;
            var name = User.Identity?.Name;

            var token = await accountApi.GetJwtToken(email, name);

            HttpContext.Session.SetString("JWT", token);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return SignOut(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }


}
