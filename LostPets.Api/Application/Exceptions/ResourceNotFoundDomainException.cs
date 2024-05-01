using Application.Exceptions.Base;

namespace Application.Exceptions
{
    public class ResourceNotFoundDomainException : BaseDomainException
    {
        public static string DefaultMessage(string entityName)
        {
            return $"The provided {entityName} entity does not exists.";
        }

        public ResourceNotFoundDomainException()
        {
        }

        public ResourceNotFoundDomainException(string? message) : base(message)
        {
        }

        public ResourceNotFoundDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
