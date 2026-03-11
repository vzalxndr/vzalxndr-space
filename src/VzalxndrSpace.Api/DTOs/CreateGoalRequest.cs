namespace VzalxndrSpace.Api.DTOs;

public record CreateGoalRequest(
    string Title,
    string? Description);