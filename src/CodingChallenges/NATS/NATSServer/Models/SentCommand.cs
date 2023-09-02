using NATSParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NATSServer.Models
{
    public record SentCommand(TcpClient Sender, INATSCommand Command);
}
