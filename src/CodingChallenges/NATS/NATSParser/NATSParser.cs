using NATSParser.Commands;
using System.Text;

namespace NATSParser
{
    public class NATSParser
    {
        public static INATSCommand Parse(string commandString)
        {
            ArgumentNullException.ThrowIfNull(commandString, nameof(commandString));
            if (string.IsNullOrWhiteSpace(commandString))
            {
                throw new ArgumentNullException(nameof(commandString));
            }

            var command = ReadCommand(commandString);

            return MapCommand(command);
        }

        private static CommandBase ReadCommand(string commandString)
        {
            var i = 0;
            var command = new CommandBase();
            var sb = new StringBuilder();
            while (i < commandString.Length && commandString[i] != ' ')
            {
                sb.Append(commandString[i]);
                i++;
            }
            command.CommandName = sb.ToString();
            if (i < commandString.Length)
            {
                command.Paramaters = commandString.Substring(i + 1, commandString.Length - i - 1);
            }
            return command;
        }

        private class CommandBase
        {
            public string CommandName { get; set; }
            public string Paramaters { get; set; }
        }

        private static INATSCommand MapCommand(CommandBase command)
        {
            switch (command.CommandName)
            {
                case NATSCommands.CONNECT: return new ConnectCommand();
                case NATSCommands.PONG: return new PongCommand();
                case NATSCommands.PING: return new PingCommand();
                case NATSCommands.SUB: return new SubCommand(command.Paramaters);
                case NATSCommands.PUB: return new PubCommand(command.Paramaters);
                default: throw new NotSupportedException($"{command} is not supported NATS command");
            }
        }
    }
}