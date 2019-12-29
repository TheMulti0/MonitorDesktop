using Microsoft.Extensions.Logging;

namespace MonitorDesktop.Extensions
{
    public class SimpleConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new SimpleConsoleLogger(categoryName);
        
        public void Dispose() { }
    }
}