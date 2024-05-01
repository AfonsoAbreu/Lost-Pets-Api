using Application.Exceptions.Base;

namespace Application.Exceptions
{
    public class MismatchedUserDomainException : BaseDomainException
    {
        public static string DefaultMessage(string entityName)
        {
            return $"The user id provided in this {entityName} entity differs from the actual user id.";
        }

        public MismatchedUserDomainException()
        {
        }

        public MismatchedUserDomainException(string? message) : base(message)
        {
        }

        public MismatchedUserDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
