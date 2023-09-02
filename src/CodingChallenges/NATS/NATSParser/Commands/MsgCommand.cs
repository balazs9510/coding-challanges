using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public class MsgCommand
    {
        public string Subject { get; set; }
        public string SId { get; set; }
        public string ReplyTo { get; set; }
        public int Bytes { get; set; }
        public string Payload { get; set; }

        public MsgCommand(PubCommand pubCommand, string sId)
        {
            Subject = pubCommand.Subject;
            SId = sId;
            Bytes = pubCommand.Bytes;
            Payload = pubCommand.Payload;
        }

        public override string ToString()
        {
            return $"MSG {Subject} {SId}{(string.IsNullOrEmpty(ReplyTo) ? "" : " ")}{ReplyTo} {Bytes}\r\n{Payload}\r\n";
        }
    }
}
