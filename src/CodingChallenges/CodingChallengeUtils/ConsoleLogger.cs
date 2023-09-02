using Microsoft.Extensions.Logging;

namespace CodingChallengeUtils
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel >= LogLevel.Error)
            {
                Console.Error.WriteLine(exception?.Message);
            }
            else
            {
                Console.WriteLine(state);
            }
        }
    }
}