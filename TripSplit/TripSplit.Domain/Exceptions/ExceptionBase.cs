using System.Net;

namespace TripSplit.Domain.Exceptions
{
    public class ExceptionBase : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public ExceptionBase(string errorMessage, HttpStatusCode httpStatusCode) : base()
        {
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }
    }
}
