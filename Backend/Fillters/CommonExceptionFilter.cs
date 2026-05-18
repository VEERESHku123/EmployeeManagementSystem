using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Backend.Fillters
{
    public class CommonExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<CommonExceptionFilter> logger;

        public CommonExceptionFilter(ILogger<CommonExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var e = context.Exception;

            logger.LogError(e, "Unhandled exception occurred: {Message}", e.Message);

            int statusCode;
            string errorMessage;

            switch(e)
            {
                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    errorMessage = e.Message;
                    break;
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

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}
