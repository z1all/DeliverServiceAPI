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

        public async Task TaskAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch
            {
                throw;
            }
        }

        private async Task HandlExceptionAsync(HttpContext httpContext, string exceptionMessage, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(exceptionMessage);

            HttpResponse response = httpContext.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDTO errorDTO = new()
            {
                Message = message,
                Status = (int)httpStatusCode
            };

            string result = JsonSerializer.Serialize(errorDTO);

            await response.WriteAsync(result);
        }
    }
}
