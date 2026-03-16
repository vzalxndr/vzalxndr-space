using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Domain.Entities;
using VzalxndrSpace.Domain.Enums;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Services;

public class SessionService : ISessionService
{
    private readonly AppDbContext _context;
    
    public SessionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SessionResponse> StartSessionAsync(StartSessionRequest request)
    {
        // Goal is required
        var goalExists = await _context.Goals.AnyAsync(g => g.Id == request.GoalId);
        if (!goalExists)
        {
            throw new ArgumentException("Goal not found.");
        }

        // check if active session exists
        var hasActiveSession = await _context.Sessions.AnyAsync(s => s.Status == SessionStatus.InProgress);
        if (hasActiveSession)
        {
            throw new InvalidOperationException("An active session already exists. Stop it first.");
        }

        var session = new Session
        {
            GoalId = request.GoalId,
            TargetDurationMinutes = request.TargetDurationMinutes,
            StartTimeUtc = DateTime.UtcNow
        };
        
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        
        return MapToResponse(session);
    }

    public async Task<SessionResponse> StopSessionAsync(Guid sessionId)
    {
        // check if session in progress exists
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            throw new ArgumentException("Session not found.");
        }
        if (session.Status != SessionStatus.InProgress)
        {
            throw new InvalidOperationException("Session is already stopped.");
        }
        
        session.EndTimeUtc = DateTime.UtcNow;
        var actualDuration = session.EndTimeUtc.Value - session.StartTimeUtc;

        if (actualDuration.TotalMinutes < 3)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Session was less than 3 minutes and was discarded.");
        }

        var gracePeriod = TimeSpan.FromSeconds(15);
        var targetDuration = TimeSpan.FromMinutes(session.TargetDurationMinutes);

        session.Status = actualDuration >= (targetDuration - gracePeriod)
            ? SessionStatus.Completed
            : SessionStatus.Interrupted;

        await _context.SaveChangesAsync();
        
        return MapToResponse(session);
    }

    // Session to DTO converter
    private static SessionResponse MapToResponse(Session session)
    {
        return new SessionResponse(
            session.Id,
            session.GoalId,
            session.StartTimeUtc,
            session.TargetDurationMinutes,
            session.Status);
    }
}