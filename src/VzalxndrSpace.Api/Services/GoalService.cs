using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Domain.Entities;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Services;

public class GoalService : IGoalService
{
    private readonly AppDbContext _context;
    
    public  GoalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GoalResponse> CreateGoalAsync(CreateGoalRequest request)
    {
        if (string.IsNullOrEmpty(request.Title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }

        var goal = new Goal
        {
            Title = request.Title,
            Description = request.Description
        };
        
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        return MapToResponse(goal);
    }

    public async Task<IEnumerable<GoalResponse>> GetGoalsAsync()
    {
        var goals = await _context.Goals
            .Where(g => g.Status != Domain.Enums.GoalStatus.Archived)
            .Include(g => g.Sessions)
            .AsNoTracking()
            .ToListAsync();

        return goals.Select(MapToResponse);
    }

    public async Task<GoalResponse> UpdateGoalAsync(Guid id, UpdateGoalRequest request)
    {
        var goal = await _context.Goals.FindAsync(id);

        if (goal == null)
        {
            throw new ArgumentException("Goal not found.");
        }
        if (string.IsNullOrEmpty(request.Title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }

        goal.Title = request.Title;
        goal.Description = request.Description;

        await _context.SaveChangesAsync();

        return MapToResponse(goal);
    }

    public async Task<GoalResponse> CompleteGoalAsync(Guid id)
    {
        var goal = await _context.Goals
            .Include(g => g.Sessions)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (goal == null)
        {
            throw new ArgumentException("Goal not found.");
        }
        if (goal.Status == Domain.Enums.GoalStatus.Completed)
        {
            throw new InvalidOperationException("Goal is already completed.");
        }
        
        goal.Status = Domain.Enums.GoalStatus.Completed;
        goal.CompletedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        return MapToResponse(goal);
    }

    public async Task<GoalResponse> ArchiveGoalAsync(Guid id)
    {
        var goal = await _context.Goals
            .Include(g => g.Sessions)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (goal == null)
        {
            throw new ArgumentException("Goal not found.");
        }
        if (goal.Status == Domain.Enums.GoalStatus.Archived)
        {
            throw new InvalidOperationException("Goal is already archived.");
        }

        goal.Status = Domain.Enums.GoalStatus.Archived;
        await _context.SaveChangesAsync();

        return MapToResponse(goal);
    }

    private static GoalResponse MapToResponse(Goal goal)
    {
        var totalMinutes = goal.Sessions
            .Where(s => s.EndTimeUtc.HasValue)
            .Sum(s => (s.EndTimeUtc!.Value - s.StartTimeUtc).TotalMinutes);

        return new GoalResponse(
            goal.Id,
            goal.Title,
            goal.Description,
            goal.CreatedAtUtc,
            goal.Status,
            goal.CompletedAtUtc,
            goal.Sessions.Count,
            (int)totalMinutes);
    }
}