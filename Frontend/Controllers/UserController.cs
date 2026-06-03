using Frontend.ApiServices.Abstracts;
using Frontend.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Frontend.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiService userApiService;
        public UserController(IUserApiService userApiService)
        {
            this.userApiService = userApiService;
        }


        [HttpGet]
        public IActionResult SignIn()
        {
            return PartialView("~/Views/User/_SignIn.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                var email = model.Email;
                
                if (!ModelState.IsValid)
                {
                    return ReturnHomeView(model,showLogin: true);
                }

                var result = await userApiService.SignIn(model);


                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty,result.Message);

                    return ReturnHomeView(model, showLogin: true);
                }

                var token = result.AuthResponse.Token;
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var employeeName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                //authenticate
                await AuthenticateUser( model.Email, result.AuthResponse.RoleType, "Native", result.AuthResponse.Token, result.AuthResponse.RefreshToken, employeeName);

                TempData["SuccessMessage"] = "SignIn successful";

                if (result.AuthResponse.RoleType == "Admin")
                    return RedirectToAction("GetAllEmployees", "Employee");
                else
                    return RedirectToAction("EmployeeDashboard", "Home");
            }
            catch (Exception e)
            {
                ModelState.AddModelError( string.Empty, e.Message);

                return ReturnHomeView(model, showLogin: true);
            }
        }

        public IActionResult MicrosoftSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/user/callback"
            };

            return Challenge(properties,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Callback()
        {
            try
            {
                var email = User.FindFirst("preferred_username")?.Value;
                
                var result = await userApiService.MicrosoftSignIn(email);

                if (result == null || !result.Success)
                {
                    TempData["ErrorMessage"] = "SignIn failed";

                    return RedirectToAction("Index", "Home");
                }

                var token = result.AuthResponse.Token;
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var employeeName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                //authenticate
                await AuthenticateUser(email, result.AuthResponse.RoleType, "Microsoft", result.AuthResponse.Token, result.AuthResponse.RefreshToken, employeeName);

                TempData["SuccessMessage"] = "SignIn successful";

                if (result.AuthResponse.RoleType == "Admin")
                    return RedirectToAction("GetAllEmployees", "Employee");
                else
                    return RedirectToAction("EmployeeDashboard", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
            

        }


        public async Task<IActionResult> SignOut()
        {
            await userApiService.SignOut();

            HttpContext.Session.Clear();


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var loginProvider = User.FindFirst("LoginProvider")?.Value;

            if (loginProvider == "Microsoft")
            {
                return SignOut(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/"
                    },
                    OpenIdConnectDefaults.AuthenticationScheme);
            }

            return RedirectToAction( "Index", "Home");
        }


        [HttpGet]
        public IActionResult ActivateAccount()
        {
            return PartialView("~/Views/User/_ActivateAccount.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccount(ActivateAccountModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ShowActivate = true;
                    return View("~/Views/Home/Index.cshtml", model);
                }

                var result = await userApiService.ActivateAccount(model);

                if (!result.Success)
                {
                    ViewBag.ShowActivate = true;

                    ModelState.AddModelError(string.Empty, result.Message);

                    return View("~/Views/Home/Index.cshtml", model);
                }

                TempData["SuccessMessage"] = result.Message;

                return RedirectToAction("_SignIn", "User", new { showLogin = true });
            }
            catch (Exception e)
            {
                ViewBag.ShowActivate = true;

                ModelState.AddModelError(string.Empty, e.Message);

                return View("~/Views/Home/Index.cshtml", model);
            }
        }




        //helper methods
        private async Task AuthenticateUser(string email, string role, string provider, string accessToken, string refreshToken, string employeeName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employeeName),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,role),
                new Claim("LoginProvider",provider)
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            HttpContext.Session.SetString("AccessToken", accessToken);

            HttpContext.Session.SetString("RefreshToken", refreshToken);
        }

        private IActionResult ReturnHomeView(object model, bool showLogin = false, bool showActivate = false)
        {
            ViewBag.ShowLogin = showLogin;
            ViewBag.ShowActivate = showActivate;

            return View("~/Views/Home/Index.cshtml", model);
        }


    }


}
