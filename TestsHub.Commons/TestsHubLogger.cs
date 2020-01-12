using Microsoft.Extensions.Logging;

namespace TestsHub.Commons
{
    public static class TestHubLogger
    {
        static readonly ILoggerFactory _loggerFactory;

        static TestHubLogger()
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole();
            });
        }

        public static ILogger<T> CreateLoger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }
    }
}
