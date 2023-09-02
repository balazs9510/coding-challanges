using CodingChallengeUtils;
using Microsoft.Extensions.Logging;
using NATSParser.Commands;
using NATSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NATSServer.Processors
{
    public class BaseCommandHandler
    {
        protected readonly ILogger<BaseCommandHandler> _logger;
        protected readonly ChannelReader<SentCommand> _channel;

        public BaseCommandHandler(ILogger<BaseCommandHandler> logger, ChannelReader<SentCommand> channel)
        {
            _logger = logger;
            _channel = channel;
        }

        public async Task StartProcessing()
        {
            await foreach (var item in _channel.ReadAllAsync())
            {
                await ProcessItem(item);
            }
        }

        public async virtual Task ProcessItem(SentCommand command)
        {
            _logger.LogInformation($"{GetType().Name} processing: {command.Command.ToString()}");
            var stream = command.Sender.GetStream();
            int a = 0;
            //stream.Dispose();
            // await stream.WriteStringAsync(new OkCommand());
        }
    }
}
