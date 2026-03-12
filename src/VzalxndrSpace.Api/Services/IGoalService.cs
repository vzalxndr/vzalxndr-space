using VzalxndrSpace.Api.DTOs;

namespace VzalxndrSpace.Api.Services;

public interface IGoalService
{
    Task<GoalResponse> CreateGoalAsync(CreateGoalRequest request);
    Task<IEnumerable<GoalResponse>> GetGoalsAsync();
    Task<GoalResponse> CompleteGoalAsync(Guid id);
}