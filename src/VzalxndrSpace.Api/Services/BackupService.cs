using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Api.Exceptions;
using VzalxndrSpace.Domain.Entities;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Services;

public class BackupService : IBackupService
{
    private readonly AppDbContext _dbContext;
    
    public BackupService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<byte[]> ExportDatabaseAsync()
    {
        var goals = await _dbContext.Goals
            .Include(g => g.Sessions)
            .AsNoTracking()
            .ToListAsync();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        
        var json = JsonSerializer.Serialize(goals, options);
        return System.Text.Encoding.UTF8.GetBytes(json);
    }

    public async Task<int> ImportDatabaseAsync(Stream fileStream)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive =  true
        };
        
        var importedGoals = await JsonSerializer.DeserializeAsync<List<Goal>>(fileStream, options);

        if (importedGoals == null || !importedGoals.Any())
        {
            throw new BadRequestException("Invalid JSON structure or empty file.");
        }
        
        _dbContext.Sessions.RemoveRange(_dbContext.Sessions);
        _dbContext.Goals.RemoveRange(_dbContext.Goals);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Goals.AddRangeAsync(importedGoals);
        await _dbContext.SaveChangesAsync();
        
        return importedGoals.Count;
    }
}