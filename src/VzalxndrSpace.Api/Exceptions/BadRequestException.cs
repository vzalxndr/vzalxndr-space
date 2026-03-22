namespace VzalxndrSpace.Api.Exceptions;

// e.g. session is already stopped/started
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}