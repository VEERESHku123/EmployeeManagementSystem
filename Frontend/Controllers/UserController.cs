using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                var result = await userService.SignIn(model);

                if (result == null)
                {
                    TempData["ErrorMessage"] = "Something went wrong";

                    return RedirectToAction("Index", "Home");
                }

                if (!result.Success)
                {
                    TempData["ErrorMessage"] = result.Message ?? "Invalid email or password";

                    return RedirectToAction("Index", "Home");
                }

                // Store JWT token
                HttpContext.Session.SetString("AccessToken", result.AuthResponse.Token);

                HttpContext.Session.SetString("RefreshToken", result.AuthResponse.RefreshToken);

                TempData["SuccessMessage"] = "SignIn successful";

                return RedirectToAction("GetAllEmployees", "Employee");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Index", "Home");
            }
            
        }

        public IActionResult MicrosoftSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/user/callback"
            };

            properties.Items["prompt"] = "select_account";

            return Challenge(properties,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Callback()
        {
            try
            {
                var email = User.FindFirst("preferred_username")?.Value;

                var auth = await userService.MicrosoftSignIn(email);

                if (auth == null || !auth.Success)
                {
                    TempData["ErrorMessage"] = "SignIn failed";

                    return RedirectToAction("Index", "Home");
                }

                HttpContext.Session.SetString("AccessToken", auth.AuthResponse.Token);

                HttpContext.Session.SetString("RefreshToken", auth.AuthResponse.RefreshToken);

                TempData["SuccessMessage"] = "SignIn successful";

                return RedirectToAction("GetAllEmployees", "Employee");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
            

        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync();

            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            OpenIdConnectDefaults.AuthenticationScheme);
        }
    }


}
