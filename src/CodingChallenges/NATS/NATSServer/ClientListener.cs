using CodingChallengeUtils;
using Microsoft.Extensions.Logging;
using NATSParser;
using NATSParser.Commands;
using NATSServer.Models;
using NATSServer.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NATSServer
{
    public class ClientListener
    {
        private readonly TcpClient _client;
        private readonly ChannelWriter<SentCommand> _channel;
        private readonly ILogger<ClientListener> _logger;
        private string _lastCommand = string.Empty;

        public ClientListener(TcpClient client,
            ILogger<ClientListener> logger,
            ChannelWriter<SentCommand> channel)
        {
            _client = client;
            _logger = logger;
            _channel = channel;
        }

        public async Task ListenAsync()
        {
            await Task.Run(async () =>
            {
                while (_client.Connected)
                {
                    var clientStream = _client.GetStream();
                    if (clientStream.DataAvailable)
                    {
                        var commandString = await clientStream.ReadStringAsync();
                        if (commandString?.StartsWith("PUB") ?? false)
                        {
                            _lastCommand = commandString;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_lastCommand))
                            {
                                commandString = $"{_lastCommand}\r\n{commandString}";
                                _lastCommand = null;
                            }
                            try
                            {
                                var command = NATSParser.NATSParser.Parse(commandString);
                                _logger.LogInformation(commandString.ToString());
                                await _channel.WriteAsync(new SentCommand(_client, command));
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, commandString);
                            }
                        }
                    }
                }

            })
           .ConfigureAwait(false);
        }
    }
}
