namespace VzalxndrSpace.Api.Services;

public interface ISessionCleanupProcessor
{
    Task ProcessAbandonedSessionAsync(CancellationToken cancellationToken);
}