using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public record SubCommand : INATSCommand
    {
        public string Subject { get; set; }
        public string? QueueGroup { get; set; }
        public string SId { get; set; }

        public SubCommand(string parameters)
        {
            var split = parameters.Split(' ');
            if (split.Length < 2)
                throw new ArgumentException($"SUB command has at least 2 required paramters");
            Subject = split[0];
            if (split.Length > 3)
            {
                QueueGroup = split[1];
                SId = split[2].TrimEnd();
            }
            else
            {
                SId = split[1].TrimEnd();
            }
        }
    }
}
