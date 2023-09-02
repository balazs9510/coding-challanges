using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public record OkCommand
    {
        public override string ToString()
        {
            return "+OK\r\n";
        }
    }
}
