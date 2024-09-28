using Infrastructure.Exceptions.Base;

namespace Infrastructure.Exceptions
{
    public class InvalidFileTypeInfrastructureException : BaseInfrastructureException
    {
        public static string DefaultMessage(string fileType)
        {
            return $"The file type \"{fileType}\" is not supported.";
        }

        public InvalidFileTypeInfrastructureException()
        {
        }

        public InvalidFileTypeInfrastructureException(string? message) : base(message)
        {
        }

        public InvalidFileTypeInfrastructureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
