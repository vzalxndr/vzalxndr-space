using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VzalxndrSpace.WpfClient.Models
{
    public record GoalDto(Guid Id, string Title, string Description, int Status);
}
