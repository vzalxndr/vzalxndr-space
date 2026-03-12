using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.DTOs.Stats;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Services;

public class StatsService : IStatsService
{
    private readonly AppDbContext _context;

    public StatsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DailyFocusDto>> GetHeatmapAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null)
    {
        var query = _context.Sessions.Where(s => s.EndTimeUtc.HasValue).AsQueryable();
        
        if (goalId.HasValue)
        {
            query = query.Where(s => s.GoalId == goalId.Value);
        }
        
        if (startDate.HasValue)
        {
            var startDateTime = startDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(s => s.StartTimeUtc >= startDateTime);
        }
        
        if (endDate.HasValue)
        {
            var endDateTime = endDate.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(s => s.StartTimeUtc <= endDateTime);
        }
        
        var rawSessions = await query
            .Select(s => new { s.StartTimeUtc, s.EndTimeUtc })
            .AsNoTracking()
            .ToListAsync();
        
        var heatmap = rawSessions
            .Select(s => new 
            {
                Date = DateOnly.FromDateTime(s.StartTimeUtc),
                Minutes = (int)(s.EndTimeUtc!.Value - s.StartTimeUtc).TotalMinutes
            })
            .GroupBy(x => x.Date)
            .Select(g => new DailyFocusDto(g.Key, g.Sum(x => x.Minutes)))
            .OrderBy(x => x.Date)
            .ToList();

        return heatmap;
    }

    public async Task<SessionStatsDto> GetSessionStatsAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null)
    {
        var query = _context.Sessions.Where(s => s.EndTimeUtc.HasValue).AsQueryable();

        if (goalId.HasValue)
            query = query.Where(s => s.GoalId == goalId.Value);

        if (startDate.HasValue)
        {
            var startDateTime = startDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(s => s.StartTimeUtc >= startDateTime);
        }

        if (endDate.HasValue)
        {
            var endDateTime = endDate.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(s => s.StartTimeUtc <= endDateTime);
        }
        
        var sessions = await query
            .Select(s => new { s.StartTimeUtc, s.EndTimeUtc, s.Status })
            .AsNoTracking()
            .ToListAsync();

        return new SessionStatsDto(
            TotalSessions: sessions.Count,
            TotalFocusMinutes: sessions.Sum(s => (int)(s.EndTimeUtc!.Value - s.StartTimeUtc).TotalMinutes),
            CompletedSessions: sessions.Count(s => s.Status == Domain.Enums.SessionStatus.Completed),
            InterruptedSessions: sessions.Count(s => s.Status == Domain.Enums.SessionStatus.Interrupted)
        );
    }
    
    public async Task<GoalStatsDto> GetGoalStatsAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null)
    {
        var createdQuery = _context.Goals.AsQueryable();
        var completedQuery = _context.Goals.Where(g => g.Status == Domain.Enums.GoalStatus.Completed).AsQueryable();

        if (goalId.HasValue)
        {
            createdQuery = createdQuery.Where(g => g.Id == goalId.Value);
            completedQuery = completedQuery.Where(g => g.Id == goalId.Value);
        }

        if (startDate.HasValue)
        {
            var startDateTime = startDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            createdQuery = createdQuery.Where(g => g.CreatedAtUtc >= startDateTime);
            completedQuery = completedQuery.Where(g => g.CompletedAtUtc >= startDateTime);
        }

        if (endDate.HasValue)
        {
            var endDateTime = endDate.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            createdQuery = createdQuery.Where(g => g.CreatedAtUtc <= endDateTime);
            completedQuery = completedQuery.Where(g => g.CompletedAtUtc <= endDateTime);
        }

        int createdCount = await createdQuery.CountAsync();
        int completedCount = await completedQuery.CountAsync();
        
        var rawGoals = await createdQuery
            .Select(g => new 
            {
                g.Id,
                g.Title,
                Sessions = g.Sessions.Where(s => s.EndTimeUtc.HasValue).Select(s => new { s.StartTimeUtc, s.EndTimeUtc })
            })
            .AsNoTracking()
            .ToListAsync();
        
        var breakdown = rawGoals.Select(g => new GoalTimeBreakdownDto(
            g.Id,
            g.Title,
            g.Sessions.Sum(s => (int)(s.EndTimeUtc!.Value - s.StartTimeUtc).TotalMinutes)
        )).ToList();

        return new GoalStatsDto(createdCount, completedCount, breakdown);
    }
}