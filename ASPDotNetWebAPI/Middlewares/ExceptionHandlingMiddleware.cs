using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Models.DTO;
using BCrypt.Net;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

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
            catch (ValidationProblemException ex)
            {
                _logger.Log(LogLevel.Information, ex, ex.Message);

                HttpResponse response = httpContext.Response;

                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                await response.WriteAsJsonAsync(ex.httpValidationProblemDetails);
            }
            catch (BadRequestException ex)
            {
                await HandlExceptionAsync(LogLevel.Information, httpContext, ex, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await HandlExceptionAsync(LogLevel.Information, httpContext, ex, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandlExceptionAsync(LogLevel.Information, httpContext, ex, HttpStatusCode.NotFound, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Operation execution error!");
            }
            catch (OperationCanceledException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "The operation was canceled!");
            }
            catch (EncoderFallbackException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Text encoding error!");
            }
            catch (ArgumentException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Unknown error!");
            }
            catch (SaltParseException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Password verification error!");
            }
            catch (DbUpdateException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "An error occurred while saving the data. Please try again later.");
            }
            catch (FormatException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Data format error");
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                await HandlExceptionAsync(LogLevel.Warning, httpContext, ex, HttpStatusCode.InternalServerError, "Error processing a secure token!");
            }
            catch (RuntimeBinderException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Unknown error!");
            }
            catch (NullReferenceException ex)
            {
                await HandlExceptionAsync(LogLevel.Error, httpContext, ex, HttpStatusCode.InternalServerError, "Unknown error!");
            }
            catch (Exception ex)
            {
                await HandlExceptionAsync(LogLevel.Critical, httpContext, ex, HttpStatusCode.InternalServerError, "Unknown error!");
                throw;
            }
        }

        private async Task HandlExceptionAsync(LogLevel logLevel, HttpContext httpContext, Exception ex, HttpStatusCode httpStatusCode, string message)
        {
            _logger.Log(logLevel, ex, ex.Message);

            HttpResponse response = httpContext.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            var errorDTO = new ResponseDTO()
            {
                Message = message,
                Status = (int)httpStatusCode
            };

            await response.WriteAsJsonAsync(errorDTO);
        }
    }
}
