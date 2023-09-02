﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSParser.Commands
{
    public class PongCommand : INATSCommand
    {
        public override string ToString()
        {
            return "PONG\r\n";
        }
    }
}
