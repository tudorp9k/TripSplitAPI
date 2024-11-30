using System.Net;

namespace TripSplit.Domain.Exceptions
{
    public class EmailTakenException : ExceptionBase
    {
        public EmailTakenException() : base("This e-mail has already been taken", HttpStatusCode.Conflict) { }
    }
}
