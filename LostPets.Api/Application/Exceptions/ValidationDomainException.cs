using Application.Exceptions.Base;

namespace Application.Exceptions
{
    public class ValidationDomainException : BaseDomainException
    {
        public ValidationDomainException()
        {
        }

        public ValidationDomainException(string? message) : base(message)
        {
        }

        public ValidationDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
