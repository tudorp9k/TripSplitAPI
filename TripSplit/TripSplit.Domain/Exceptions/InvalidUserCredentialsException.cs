using System.Net;

namespace TripSplit.Domain.Exceptions
{
    public class InvalidUserCredentialsException : ExceptionBase
    {
        public InvalidUserCredentialsException(string message) : base(message, HttpStatusCode.Unauthorized) { }
        public InvalidUserCredentialsException() : base("Invalid user credentials", HttpStatusCode.Unauthorized) { }
    }
}
