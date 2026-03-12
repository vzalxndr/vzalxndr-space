using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.DTOs.Stats;

namespace VzalxndrSpace.Api.Services;

public interface IStatsService
{
    Task<IEnumerable<DailyFocusDto>> GetHeatmapAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null);
    
    Task<SessionStatsDto> GetSessionStatsAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null);
    
    Task<GoalStatsDto> GetGoalStatsAsync(
        Guid? goalId = null, 
        DateOnly? startDate = null, 
        DateOnly? endDate = null);
}