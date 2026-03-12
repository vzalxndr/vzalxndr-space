using System;
using VzalxndrSpace.Domain.Enums;

namespace VzalxndrSpace.Api.DTOs;

public record GoalResponse(
    Guid Id,
    string Title,
    string? Description,
    DateTime CreatedAtUtc,
    GoalStatus Status,
    DateTime? CompletedAtUtc,
    int SessionsCount,
    int TotalTimeSpentMinutes
);