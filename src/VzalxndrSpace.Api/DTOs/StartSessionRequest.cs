namespace VzalxndrSpace.Api.DTOs;

public record StartSessionRequest (
    Guid GoalId, 
    int TargetDurationMinutes);