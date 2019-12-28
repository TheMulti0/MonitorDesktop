using Microsoft.Extensions.Logging;

namespace MonitorDesktop.Extensions
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory CreateDefault()
            => LoggerFactory.Create(builder => { });
    }
}