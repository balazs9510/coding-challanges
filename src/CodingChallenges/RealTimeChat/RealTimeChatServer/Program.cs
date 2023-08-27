// Step 1: build an Echo Server; RFC-862: https://datatracker.ietf.org/doc/html/rfc862
using RealTimeChatServer;
var server = new ServerProcess(new ConsoleLogger<ServerProcess>());
var tokenSrc = new CancellationTokenSource();
var cancellationToken = tokenSrc.Token;
var serverTask = server.RunServer(cancellationToken);
string input = Console.ReadLine();
while(input != null)
{
    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
    {
        tokenSrc.Cancel();
        Console.WriteLine("App will exit soon..");
        Task.Delay(500).Wait();
        Environment.Exit(0);
    }
    input = Console.ReadLine();
}
Task.WaitAll(serverTask);