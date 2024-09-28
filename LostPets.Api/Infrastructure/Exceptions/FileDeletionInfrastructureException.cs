using Infrastructure.Exceptions.Base;

namespace Infrastructure.Exceptions
{
    public class FileDeletionInfrastructureException : BaseInfrastructureException
    {
        public static string DefaultMessage(string filePath)
        {
            return $"Could not delete the file located at {filePath}.";
        }

        public FileDeletionInfrastructureException()
        {
        }

        public FileDeletionInfrastructureException(string? message) : base(message)
        {
        }

        public FileDeletionInfrastructureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
