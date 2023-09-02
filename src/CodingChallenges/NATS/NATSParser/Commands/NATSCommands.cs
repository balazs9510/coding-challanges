using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public static class NATSCommands
    {
        public const string CONNECT = nameof(CONNECT);
        public const string PING = nameof(PING);
        public const string PONG = nameof(PONG);
        public const string SUB = nameof(SUB);
        public const string PUB = nameof(PUB);
    }
}
