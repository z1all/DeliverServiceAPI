using ASPDotNetWebAPI.Models.DTO;
using System.Net;
using System.Text.Json;

namespace ASPDotNetWebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandlExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError, "Error");
                throw;
            }
        }

        private async Task HandlExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(ex, ex.Message);

            HttpResponse response = httpContext.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDTO errorDTO = new()
            {
                Message = message,
                Status = (int)httpStatusCode
            };

            await response.WriteAsJsonAsync(errorDTO);
        }
    }
}
