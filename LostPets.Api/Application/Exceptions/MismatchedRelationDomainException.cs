using Application.Exceptions.Base;

namespace Application.Exceptions
{
    public class MismatchedRelationDomainException : BaseDomainException
    {
        public static string DefaultMessage(string entityName, string propertyName)
        {
            return $"The provided {propertyName} property in this {entityName} entity differs from the actual {propertyName}.";
        }

        public MismatchedRelationDomainException()
        {
        }

        public MismatchedRelationDomainException(string? message) : base(message)
        {
        }

        public MismatchedRelationDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
