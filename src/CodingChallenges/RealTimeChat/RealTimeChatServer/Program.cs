// Step 1: build an Echo Server; RFC-862: https://datatracker.ietf.org/doc/html/rfc862
using RealTimeChatServer;
var server = new ServerProcess(new ConsoleLogger<ServerProcess>());
await server.RunServer();
Console.WriteLine("Server exit.");