using System;

namespace VzalxndrSpace.Api.DTOs;

public record GoalResponse(
    Guid Id,
    string Title,
    string? Description,
    DateTime CreatedAtUtc,
    bool IsActive,
    int SessionsCount,
    int TotalTimeSpentMinutes
);