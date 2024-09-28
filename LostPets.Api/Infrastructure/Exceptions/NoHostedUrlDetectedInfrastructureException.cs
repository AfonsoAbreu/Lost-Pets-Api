using Infrastructure.Exceptions.Base;

namespace Infrastructure.Exceptions
{
    public class NoHostedUrlDetectedInfrastructureException : BaseInfrastructureException
    {
        public static string DefaultMessage()
        {
            return $"Could not detect an URL for the host.";
        }

        public NoHostedUrlDetectedInfrastructureException()
        {
        }

        public NoHostedUrlDetectedInfrastructureException(string? message) : base(message)
        {
        }

        public NoHostedUrlDetectedInfrastructureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
