using VzalxndrSpace.Api.Services;

namespace VzalxndrSpace.Api.BackgroundServices;

public class SessionCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SessionCleanupWorker> _logger;

    public SessionCleanupWorker(IServiceProvider serviceProvider, ILogger<SessionCleanupWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Session cleanup background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var processor = scope.ServiceProvider.GetRequiredService<ISessionCleanupProcessor>();

                await processor.ProcessAbandonedSessionAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error while cleaning session.");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}