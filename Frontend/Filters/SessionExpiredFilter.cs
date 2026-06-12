using Frontend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Frontend.Filters
{
    public class SessionExpiredFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is SessionExpiredException)
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Home",
                    new { showLogin = true });

                context.ExceptionHandled = true;
            }
        }
    }
}
