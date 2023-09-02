using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatServer
{
    public static class Extensions
    {
        public static async Task<string> ReadStringAsync(this NetworkStream stream)
        {
            var sb = new StringBuilder();
            var buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                sb.Append(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            }
            return sb.ToString();
        }
    }
}
