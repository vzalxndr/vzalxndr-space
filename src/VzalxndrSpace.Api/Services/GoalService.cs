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
            .Include(g => g.Sessions)
            .AsNoTracking()
            .ToListAsync();
        
        return goals.Select(MapToResponse);
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
            goal.IsActive,
            goal.Sessions.Count,
            (int)totalMinutes);
    }
}