using CodingChallengeUtils;
using NATSServer;

var server = new ServerProcess(new ConsoleLogger<ServerProcess>(), port: 4222);
var serverTask = server.RunServer(new CancellationTokenSource().Token);

await Task.WhenAll(serverTask);