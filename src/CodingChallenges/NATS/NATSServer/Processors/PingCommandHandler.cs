using CodingChallengeUtils;
using Microsoft.Extensions.Logging;
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
    public class PingCommandHandler : BaseCommandHandler
    {
        public PingCommandHandler(ILogger<BaseCommandHandler> logger, ChannelReader<SentCommand> channel) : base(logger, channel)
        {
        }

        public override Task ProcessItem(SentCommand command)
        {
            return command.Sender.GetStream().WriteStringAsync(new PongCommand());
        }
    }
}
