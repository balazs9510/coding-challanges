using RealTimeChatServer.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatServer.Implementations
{
    public class ClientRepository : IClientRepository
    {
        public ConcurrentBag<TcpClient> Clients { get; set; }

        private static ClientRepository _instance;

        public static ClientRepository Instance
        {
            get
            {
                if (_instance == null) _instance = new ClientRepository();
                return _instance;
            }
        }

        private ClientRepository() { }

        public void AddClient(TcpClient client) { Clients.Add(client); }

        public List<TcpClient> GetConnectedClients()
        {
            return Clients.Where(c => c.Connected).ToList();
        }
    }
}
