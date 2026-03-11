using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VzalxndrSpace.Domain.Entities;
using VzalxndrSpace.Infrastructure.Data;

namespace VzalxndrSpace.Api.Controllers;

//TODO: move logic to GoalService!!!
[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GoalsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title cannot be empty");

        var goal = new Goal { Title = title };
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        return Ok(goal);
    }

    [HttpGet]
    public async Task<IActionResult> GetGoals()
    {
        // .INCLUDE ONLY FOR DEVELOPMENT
        var goals = await _context.Goals
            .Include(g => g.Sessions)
            .ToListAsync();
        
        return Ok(goals);
    }
}