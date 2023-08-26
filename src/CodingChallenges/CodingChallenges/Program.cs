// Step 1: build an Echo Server; RFC-862: https://datatracker.ietf.org/doc/html/rfc862

using RealTimeChatServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

var port = 7007;
var localAddr = IPAddress.Parse("127.0.0.1");


var server = new ServerProcess(new ConsoleLogger<ServerProcess>());

var task = server.RunServer();

var client = new TcpClient("127.0.0.1", 7007);

using var stream = client.GetStream();
var msg = Encoding.UTF8.GetBytes("Helloka");
stream.Write(msg, 0, msg.Length);

await Task.WhenAll(task);

Console.ReadKey();
//try
//{
//    var server  = new TcpListener(localAddr, port);
//    server.Start();
//    Console.WriteLine("Server started on: {0}:{1}", localAddr, port);
//    Byte[] bytes = new Byte[256];
//    String data = null;
   
//    while (true)
//    {
//        Console.WriteLine("Waiting for a connection... ");
//        using var client = server.AcceptTcpClient();
//        Console.WriteLine("Client connected: {0}", client);
//        data = null;
//        var stream = client.GetStream();
//        int i;
//        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
//        {
//            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
//            Console.WriteLine("Received: {0}", data);
//            data = data.ToUpper();
//            var msg = System.Text.Encoding.ASCII.GetBytes(data);
//            stream.Write(msg, 0, msg.Length);
//            Console.WriteLine("Sent: {0}", data);
//        }
//    }
//}
//catch (Exception e)
//{
//    await Console.Error.WriteLineAsync(e.Message);
//}

// New-Object System.Net.Sockets.TcpClient("127.0.0.1", 7007) 