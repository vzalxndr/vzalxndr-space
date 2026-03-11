using VzalxndrSpace.Api.DTOs;

namespace VzalxndrSpace.Api.Services;

public interface ISessionService
{
    Task<SessionResponse> StartSessionAsync(StartSessionRequest startSessionRequest);
    Task<SessionResponse> StopSessionAsync(Guid sessionId);
}