using VzalxndrSpace.Domain.Enums;

namespace VzalxndrSpace.Domain.Entities;

public class Session
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    // foreign key mapping to the parent Goal
    public Guid GoalId { get; set; }
    
    // target duration provided by the client (e.g., 25 mins) to calculate completion status
    public int TargetDurationMinutes { get; set; }
    
    public DateTime StartTimeUtc { get; set; }
    
    // nullable because it remains unassigned until the session is stopped
    public DateTime? EndTimeUtc { get; set; }
    
    public SessionStatus Status { get; set; } = SessionStatus.InProgress;
    
    // navigation property mapping back to the parent Goal
    public Goal?  Goal { get; set; }
}