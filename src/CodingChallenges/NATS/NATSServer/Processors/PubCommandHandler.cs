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
    public class PubCommandHandler : BaseCommandHandler
    {
        public PubCommandHandler(ILogger<PubCommandHandler> logger, ChannelReader<SentCommand> channel) : base(logger, channel)
        {
        }

        public async override Task ProcessItem(SentCommand command)
        {
            var pubCommand = command.Command as PubCommand;
            if (SubProvider.Instance.Subscriptions.TryGetValue(pubCommand.Subject, out var clients))
            {
                Parallel.ForEach(clients, async (c) =>
                {
                    var stream = c.Client.GetStream();
                    await stream.WriteStringAsync(new MsgCommand(pubCommand, c.SId));
                });
            }
            await base.ProcessItem(command);
        }
    }
}
