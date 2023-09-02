using CodingChallengeUtils;
using Microsoft.Extensions.Logging;
using NATSParser.Commands;
using NATSServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NATSServer.Processors
{
    public class SubCommandHandler : BaseCommandHandler
    {
        public SubCommandHandler(ILogger<SubCommandHandler> logger, ChannelReader<SentCommand> channel) : base(logger, channel) { }

        public override Task ProcessItem(SentCommand command)
        {
            _logger.LogInformation($"Thread: {Thread.CurrentThread.ManagedThreadId} processing sub");
            var subCommand = command.Command as SubCommand;
            var subject = subCommand!.Subject;

            if (SubProvider.Instance.Subscriptions.TryGetValue(subject, out var tcpClients))
            {
                tcpClients.Add(new ClientSub(command.Sender, subCommand.SId));
            }
            else
            {
                SubProvider.Instance.Subscriptions.TryAdd(subject, new List<ClientSub> { new ClientSub(command.Sender, subCommand.SId) });
            }
            return base.ProcessItem(command);

        }
    }
}
