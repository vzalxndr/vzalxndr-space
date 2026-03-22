using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VzalxndrSpace.Api.DTOs;
using VzalxndrSpace.Api.Services;

namespace VzalxndrSpace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;
    
    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionRequest request)
    {
        var result = await _sessionService.StartSessionAsync(request);
        return Ok(result);
    }

    [HttpPost("{id:guid}/stop")]
    public async Task<IActionResult> StopSession(Guid id)
    {
        var result = await _sessionService.StopSessionAsync(id);
        return Ok(result);
    }
}