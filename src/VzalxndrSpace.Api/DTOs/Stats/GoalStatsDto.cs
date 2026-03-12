using System;
using System.Collections.Generic;

namespace VzalxndrSpace.Api.DTOs.Stats;

public record GoalStatsDto(
    int TotalGoalsCreated,
    int TotalGoalsCompleted,
    IEnumerable<GoalTimeBreakdownDto> GoalsBreakdown
);

public record GoalTimeBreakdownDto(
    Guid GoalId,
    string Title,
    int TotalMinutes
);