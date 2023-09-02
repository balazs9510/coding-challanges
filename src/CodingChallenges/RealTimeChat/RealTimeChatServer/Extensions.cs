using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatServer
{
    public static class Extensions
    {
        public static async Task<string> ReadStringAsync(this NetworkStream stream)
        {
            var sb = new StringBuilder();
            //var buffer = new byte[1024];
            //int bytesRead;
            //await stream.ReadAsync(buffer, 0, buffer.Length);
            ////while ((bytesRead = ) > 0)
            ////{
            //    sb.Append(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            ////}
            StreamReader reader = new StreamReader(stream);
            String line = reader.ReadLine();
            return line;
        }
    }
}
