using VzalxndrSpace.Domain.Enums;

namespace VzalxndrSpace.Domain.Entities;

public class Goal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    // enforces UTC time at the domain level per architecture rules
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    
    public DateTime? CompletedAtUtc { get; set; }
    
    // navigation property: one Goal contains multiple Sessions
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}