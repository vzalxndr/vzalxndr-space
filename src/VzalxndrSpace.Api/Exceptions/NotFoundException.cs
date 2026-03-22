namespace VzalxndrSpace.Api.Exceptions;

// e.g. db doesn't contain requested session or goal
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}