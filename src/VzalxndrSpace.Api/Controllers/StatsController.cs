using Microsoft.AspNetCore.Mvc;
using VzalxndrSpace.Api.Services;

namespace VzalxndrSpace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;
    
    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("heatmap")]
    public async Task<IActionResult> GetHeatmap(
        [FromQuery] Guid? goalId,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate)
    {
        var result = await _statsService.GetHeatmapAsync(goalId,  startDate, endDate);
        return Ok(result);
    }
    
    [HttpGet("sessions-summary")]
    public async Task<IActionResult> GetSessionStats(
        [FromQuery] Guid? goalId,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate)
    {
        var result = await _statsService.GetSessionStatsAsync(goalId, startDate, endDate);
        return Ok(result);
    }

    [HttpGet("goals-summary")]
    public async Task<IActionResult> GetGoalStats(
        [FromQuery] Guid? goalId,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate)
    {
        var result = await _statsService.GetGoalStatsAsync(goalId, startDate, endDate);
        return Ok(result);
    }
}