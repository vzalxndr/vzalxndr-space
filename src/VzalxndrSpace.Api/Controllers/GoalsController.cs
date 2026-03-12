using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.Services;
using VzalxndrSpace.Domain.Entities;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly IGoalService _goalService;

    public GoalsController(IGoalService goalService)
    {
        _goalService = goalService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest request)
    {
        try
        {
            var result = await _goalService.CreateGoalAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetGoals()
    {
        var result = await _goalService.GetGoalsAsync();
        return Ok(result);
    }
    
    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> CompleteGoal(Guid id)
    {
        try
        {
            var result = await _goalService.CompleteGoalAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            // invalid request data (e.g.: Goal doesn't exist)
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // invalid operation, Goal is already completed
            return Conflict(new { message = ex.Message });
        }
    }
}