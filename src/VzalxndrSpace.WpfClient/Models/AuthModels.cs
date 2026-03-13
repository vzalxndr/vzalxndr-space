using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VzalxndrSpace.WpfClient
{
    public record LoginRequest(string Password);
    public record AuthResponse(string Token);

}
