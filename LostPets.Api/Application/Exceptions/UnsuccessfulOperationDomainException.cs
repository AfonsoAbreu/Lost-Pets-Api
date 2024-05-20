using Application.Exceptions.Base;

namespace Application.Exceptions
{
    public class UnsuccessfulOperationDomainException : BaseDomainException
    {
        public static string DefaultMessage(string operationName)
        {
            return $"The operation \"{operationName}\" failed.";
        }

        public UnsuccessfulOperationDomainException()
        {
        }

        public UnsuccessfulOperationDomainException(string? message) : base(message)
        {
        }

        public UnsuccessfulOperationDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
