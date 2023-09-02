using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public class PubCommand : INATSCommand
    {
        public string Subject { get; set; }
        public string ReplyTo { get; set; }
        public int Bytes { get; set; }
        public string Payload { get; set; }

        public PubCommand(string parameters)
        {
            var splitEnd = parameters.Split(Environment.NewLine);
            Payload = splitEnd[1].Trim();
            var splitStart = splitEnd[0].Split(" ");
            Subject = splitStart[0];
            if (splitStart.Length > 2)
            {
                ReplyTo= splitStart[1];
                Bytes = int.Parse(splitStart[2]);
            }
            else
            {
                Bytes = int.Parse(splitStart[1]);
            }
        }
    }
}
