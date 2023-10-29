namespace ASPDotNetWebAPI.Exceptions
{
    public class ValidationProblemException : Exception
    {
        public ValidationProblemException() { }
        public ValidationProblemException(string message) : base(message) { }
        public ValidationProblemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
