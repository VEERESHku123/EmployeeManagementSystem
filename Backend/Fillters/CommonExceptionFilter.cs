using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Auth.Fillters
{
    public class CommonExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var e = context.Exception;

            int statusCode;
            string errorMessage;

            switch(e)
            {
                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = e.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    errorMessage = e.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = e.Message;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Something Went Wrong.Please Try Some Other Time";
                    break;
            }

            var errorResponse = new
            {
                StatusCode = statusCode,
                Error = errorMessage
            };

            context.Result = new JsonResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
