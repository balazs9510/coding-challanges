using System.Net.Sockets;
using System.Text;

Console.WriteLine("[chatclient c#] Hello!");
Console.WriteLine("[chatclient c#] Please add your name:");
var userName = Console.ReadLine();

TcpClient client = null;
NetworkStream stream = null;
try
{
    client = new TcpClient("127.0.0.1", 7007);
    stream = client.GetStream();
}
catch
{
    Console.Error.WriteLine("Error connecting to server");
    Environment.Exit(-1);
}
var messageQueue = new Queue<string>();
Task.Run(async () =>
{
    
    while (client.Connected)
    {
        if (stream.DataAvailable)
        {
            var incomingStream = new byte[client.ReceiveBufferSize];
            await stream.ReadAsync(incomingStream, 0, incomingStream.Length);

            Console.WriteLine(Encoding.UTF8.GetString(incomingStream));

        }

        while (messageQueue.Any())
        {
            var msg = messageQueue.Dequeue();
            var byteMsg = Encoding.UTF8.GetBytes($">>{userName} says: {msg}");
            stream.Write(byteMsg, 0, byteMsg.Length);
            stream.Flush();
        }    
    }
});


Console.WriteLine("[chatclient c#] You can start to type your messages.");

string? message = Console.ReadLine();
messageQueue.Enqueue(message);
while (!string.Equals((message), "exit", StringComparison.OrdinalIgnoreCase) && message != null)
{
    message = Console.ReadLine();
    messageQueue.Enqueue(message);
}

