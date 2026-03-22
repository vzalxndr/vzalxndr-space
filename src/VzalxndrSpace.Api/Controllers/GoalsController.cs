using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.Services;

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
    [Authorize]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest request)
    {
        var result = await _goalService.CreateGoalAsync(request);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalRequest request)
    {
        var result = await _goalService.UpdateGoalAsync(id, request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetGoals()
    {
        var result = await _goalService.GetGoalsAsync();
        return Ok(result);
    }
    
    [HttpPatch("{id:guid}/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteGoal(Guid id)
    {
        var result = await _goalService.CompleteGoalAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/archive")]
    [Authorize]
    public async Task<IActionResult> ArchiveGoal(Guid id)
    {
        var result = await _goalService.ArchiveGoalAsync(id);
        return Ok(result);
    }
}