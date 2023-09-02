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
    public class ConnectCommandHandler : BaseCommandHandler
    {
        public ConnectCommandHandler(ILogger<BaseCommandHandler> logger, ChannelReader<SentCommand> channel) : base(logger, channel)
        {
        }

        public override Task ProcessItem(SentCommand command)
        {
            //var stream = command.Sender.GetStream();
            //await stream.
            //    WriteStringAsync(new PingCommand());
            return base.ProcessItem(command);
        }
    }
}
