namespace Infrastructure.Exceptions.Base
{
    public abstract class BaseInfrastructureException : Exception
    {
        public BaseInfrastructureException()
        {
        }

        public BaseInfrastructureException(string? message) : base(message)
        {
        }

        public BaseInfrastructureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
