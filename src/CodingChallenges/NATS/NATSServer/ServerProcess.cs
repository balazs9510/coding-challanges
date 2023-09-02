using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CodingChallengeUtils;
using NATSParser;
using NATSServer.Processors;
using System.Threading.Channels;
using NATSServer.Models;
using NATSParser.Commands;
using System.IO;
using static NATSParser.Commands.InfoCommand;

namespace NATSServer
{
    public class ServerProcess
    {
        private readonly TcpListener _server;
        private readonly ILogger<ServerProcess> _logger;
        private readonly Dictionary<Type, Channel<SentCommand>> _commandTypes = new[]
            {
                typeof(ConnectCommand), typeof(PubCommand), typeof(PingCommand), typeof(SubCommand), typeof(PongCommand)
            }.ToDictionary(x => x, y => Channel.CreateUnbounded<SentCommand>());

        public ServerProcess(ILogger<ServerProcess> logger, string address = "0.0.0.0", int port = 7007)
        {
            var localAddr = IPAddress.Parse(address);
            _server = new TcpListener(localAddr, port);
            _logger = logger;
        }

        public async Task RunServer(CancellationToken cancellationToken)
        {
            try
            {
                _server.Start();
                _logger.LogInformation($"Server started on: {_server.LocalEndpoint}");
                Channel<SentCommand> mainChannel = StartCommandProcessors();

                await Task.Run(async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (_server.Pending())
                        {
                            var currentClient = await _server.AcceptTcpClientAsync();
                            var infoCommand = new InfoCommand
                            {

                                Setting = new Settings
                                {
                                    host = "0.0.0.0",
                                    port = 4222,
                                    max_payload = 1024,
                                    proto= 0,
                                    version = "1.19.0",
                                    client_id = 1000
                                }
                            };
                            await currentClient.GetStream().WriteStringAsync(infoCommand);

                            var listener = new ClientListener(currentClient, new ConsoleLogger<ClientListener>(), mainChannel.Writer);
                            _logger.LogInformation("Client connected.");

                            Task.Run(() => { listener.ListenAsync(); });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occured.");
            }
            finally
            {
                _server.Stop();
                _logger.LogInformation($"Server stopped.");
            }
        }

        private Channel<SentCommand> StartCommandProcessors()
        {
            var mainChannel = Channel.CreateBounded<SentCommand>(100_000);
            var commandChannelWriters = _commandTypes.ToDictionary(x => x.Key, y => y.Value.Writer);
            var commandProcessor = new CommandProcessor(new ConsoleLogger<CommandProcessor>(), mainChannel.Reader, commandChannelWriters);
            _ = commandProcessor.StartProcessing();

            var connectionProcessor = new ConnectCommandHandler(new ConsoleLogger<ConnectCommandHandler>(), _commandTypes[typeof(ConnectCommand)].Reader);
            _ = connectionProcessor.StartProcessing();

            for (int i = 0; i < 3; i++)
            {
                var subProcessor = new SubCommandHandler(new ConsoleLogger<SubCommandHandler>(), _commandTypes[typeof(SubCommand)].Reader);
                _ = subProcessor.StartProcessing();
            }

            for (int i = 0; i < 3; i++)
            {
                var pubProcessor = new PubCommandHandler(new ConsoleLogger<PubCommandHandler>(), _commandTypes[typeof(PubCommand)].Reader);
                _ = pubProcessor.StartProcessing();
            }
            var pingProcessor = new PingCommandHandler(new ConsoleLogger<PingCommandHandler>(), _commandTypes[typeof(PingCommand)].Reader);
            _ = pingProcessor.StartProcessing();
            var pongProcessor = new PongCommandHandler(new ConsoleLogger<PongCommandHandler>(), _commandTypes[typeof(PongCommand)].Reader);
            _ = pongProcessor.StartProcessing();
            return mainChannel;
        }
    }
}
