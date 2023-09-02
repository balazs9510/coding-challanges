using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallengeUtils
{
    public static class NetworkStreamExtensions
    {
        public static async Task<string> ReadStringAsync(this NetworkStream stream)
        {
            //var reader = new StreamReader(stream);
            //return await reader!.ReadLineAsync().ConfigureAwait(false);
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

            // Read all the data until the end of stream has been reached.
            // The incoming message may be larger than the buffer size.
            while (numberOfBytesRead > 0)
            {
                myCompleteMessage.Append(Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
            }

            return myCompleteMessage.ToString();
        }

        public static async Task WriteStringAsync(this NetworkStream stream, string data)
        {
            var reader = new StreamWriter(stream);
            reader.AutoFlush= true;
            reader.Write(data);
            reader.Flush();
            //await reader!.WriteAsync(data).ConfigureAwait(false);
            // await reader.FlushAsync();
        }

        public static async Task WriteStringAsync(this NetworkStream stream, object data)
        {
            await stream.WriteStringAsync(data?.ToString());
        }
    }
}
