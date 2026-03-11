using VzalxndrSpace.Domain.Enums;

namespace VzalxndrSpace.Api.DTOs;

public record SessionResponse(
    Guid SessionId,
    Guid GoalId,
    DateTime StartTimeUtc,
    int TargetDurationMinutes,
    SessionStatus Status);