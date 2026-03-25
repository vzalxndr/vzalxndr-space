namespace VzalxndrSpace.Api.Services;

public interface IBackupService
{
    Task<byte[]> ExportDatabaseAsync();
    Task<int> ImportDatabaseAsync(Stream fileStream);
}