using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetWebAPI.Exceptions
{
    public class ValidationProblemException : Exception
    {
        public HttpValidationProblemDetails httpValidationProblemDetails;

        public ValidationProblemException(string key, string message) : base(message)
        {
            httpValidationProblemDetails = new HttpValidationProblemDetails();
            httpValidationProblemDetails.Errors.Add(key, new[] { message });
        }

        public ValidationProblemException(HttpValidationProblemDetails httpValidationProblemDetails) : base("Any errors in model")
        {
            this.httpValidationProblemDetails = httpValidationProblemDetails;
        }
    }
}
