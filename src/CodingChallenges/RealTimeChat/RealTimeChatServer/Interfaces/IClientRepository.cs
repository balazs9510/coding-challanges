using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatServer.Interfaces
{
    public interface IClientRepository
    {
        void AddClient(TcpClient client);
    }
}
