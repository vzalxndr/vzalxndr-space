using Microsoft.AspNetCore.Mvc;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.Services;

namespace VzalxndrSpace.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;
    
    public  SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionRequest request)
    {
        try
        {
            var result = await _sessionService.StartSessionAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            // invalid request data (e.g.: Goal doesn't exist)
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // an active session already exists
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/stop")]
    public async Task<IActionResult> StopSession(Guid id)
    {
        try
        {
            var result = await _sessionService.StopSessionAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            // invalid request data (e.g.: Session doesn't exist)
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // session is already stopped
            return Conflict(new { message = ex.Message });
        }
    }
}