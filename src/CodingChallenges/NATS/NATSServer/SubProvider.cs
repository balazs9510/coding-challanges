using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NATSServer
{
    public record ClientSub(TcpClient Client, string SId);

    public class SubProvider
    {
        public readonly ConcurrentDictionary<string, List<ClientSub>> Subscriptions = new ConcurrentDictionary<string, List<ClientSub>>();
           
        private static SubProvider instance = null;
        private static readonly object padlock = new object();
        public static SubProvider Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SubProvider();
                    }
                    return instance;
                }
            }
        }
    }
}
