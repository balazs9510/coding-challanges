using NATSParser.Commands;

namespace NATSParser.Tests
{
    public class NATSParserTests
    {
        [Fact]
        public void CanParseConnectCommand()
        {
            // Arrange
            // Act
            var connectCommand = NATSParser.Parse("CONNECT");

            // Assert
            Assert.True(connectCommand is ConnectCommand);
        }

        [Fact]
        public void CanParsePongCommand()
        {
            // Arrange
            // Act
            var connectCommand = NATSParser.Parse("PONG");

            // Assert
            Assert.True(connectCommand is PongCommand);
        }

        [Fact]
        public void CanParsePingCommand()
        {
            // Arrange
            // Act
            var connectCommand = NATSParser.Parse("PING");

            // Assert
            Assert.True(connectCommand is PingCommand);
        }

        [Fact]
        public void CanParseSubCommand()
        {
            // Arrange
            var commandString = "SUB FOO 1\r\n";

            // Act
            var command = NATSParser.Parse(commandString);

            // Assert
            Assert.True(command is SubCommand);
            var subCommand = command as SubCommand;
            Assert.Equal("FOO" , subCommand.Subject);
            Assert.Equal("1", subCommand.SId);
        }

        [Fact]
        public void CanParsePubCommand()
        {
            // Arrange
            var commandString = "PUB CodingChallenge 11\r\nHello John!\r\n";

            // Act
            var command = NATSParser.Parse(commandString);

            // Assert
            Assert.True(command is PubCommand);
            var pubCommand = command as PubCommand;
            Assert.Equal("CodingChallenge", pubCommand.Subject);
            Assert.Equal(11, pubCommand.Bytes);
            Assert.Equal("Hello John!", pubCommand.Payload);
        }
    }
}