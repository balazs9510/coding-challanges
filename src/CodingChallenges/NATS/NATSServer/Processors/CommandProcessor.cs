using CodingChallengeUtils;
using Microsoft.Extensions.Logging;
using NATSParser;
using NATSParser.Commands;
using NATSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NATSServer.Processors
{
    public class CommandProcessor
    {
        private readonly ILogger _logger;
        private readonly ChannelReader<SentCommand> _channel;
        private readonly Dictionary<Type, ChannelWriter<SentCommand>> _mapping;

        public CommandProcessor(ILogger logger, ChannelReader<SentCommand> channel, Dictionary<Type, ChannelWriter<SentCommand>> mapping)
        {
            _logger = logger;
            _channel = channel;
            _mapping = mapping;
        }

        public async Task StartProcessing()
        {
            await foreach (var item in _channel.ReadAllAsync())
            {
                var handler = MapHandler(item);
                await handler.WriteAsync(item);
            }
        }


        private ChannelWriter<SentCommand> MapHandler(SentCommand sent)
        {
           return _mapping[sent.Command.GetType()];
        }
    }
}
