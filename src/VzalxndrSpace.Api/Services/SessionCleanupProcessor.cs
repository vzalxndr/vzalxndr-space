using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Domain.Enums;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Services;

public class SessionCleanupProcessor : ISessionCleanupProcessor
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SessionCleanupProcessor> _logger;

    public SessionCleanupProcessor(AppDbContext dbContext, ILogger<SessionCleanupProcessor> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ProcessAbandonedSessionAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var activeSession = await _dbContext.Sessions
            .Where(s => s.Status == SessionStatus.InProgress)
            .ToListAsync(cancellationToken);

        var abandonedCount = 0;

        foreach (var session in activeSession)
        {
            var expectedEndTime = session.StartTimeUtc.AddMinutes(session.TargetDurationMinutes);

            if (now >= expectedEndTime.AddMinutes(15))
            {
                session.Status = SessionStatus.Interrupted;
                session.EndTimeUtc = expectedEndTime;
                
                abandonedCount++;
                _logger.LogInformation($"Session {session.Id} closed automatically (timeout).");
            }
        }

        if (abandonedCount > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Successfully cleaned  up {abandonedCount} abandoned sessions.");
        }
    }
}