using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VzalxndrSpace.Domain.Enums;

namespace VzalxndrSpace.WpfClient.Models
{
    public record SessionResponse(
        Guid SessionId,
        Guid GoalId,
        DateTime StartTimeUtc,
        int TargetDurationMinutes,
        SessionStatus Status);
}
