namespace Application.Exceptions.Base
{
    public abstract class BaseDomainException : Exception
    {
        public BaseDomainException()
        {
        }

        public BaseDomainException(string? message) : base(message)
        {
        }

        public BaseDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
