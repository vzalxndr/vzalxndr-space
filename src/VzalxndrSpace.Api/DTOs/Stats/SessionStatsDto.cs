namespace VzalxndrSpace.Api.DTOs;

public record SessionStatsDto(
    int TotalSessions,
    int TotalFocusMinutes,
    int CompletedSessions,
    int InterruptedSessions
);