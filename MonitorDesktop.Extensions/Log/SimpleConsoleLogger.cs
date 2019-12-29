using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MonitorDesktop.Extensions
{
    public class SimpleConsoleLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Dictionary<LogLevel, ConsoleColor> _levelColors;

        public SimpleConsoleLogger(string categoryName)
        {
            _categoryName = categoryName;
            _levelColors = new Dictionary<LogLevel, ConsoleColor>()
            {
                { LogLevel.Critical, ConsoleColor.Red },
                { LogLevel.Error, ConsoleColor.DarkRed },
                { LogLevel.Warning, ConsoleColor.Yellow },
                { LogLevel.Information, ConsoleColor.White },
                { LogLevel.Debug, ConsoleColor.Green },
                { LogLevel.Trace, ConsoleColor.DarkGray },
            };
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (logLevel != LogLevel.None)
            {
                Console.ForegroundColor = _levelColors[logLevel];
            }
            Console.WriteLine($"{DateTime.Now}: {logLevel}: {_categoryName}[{eventId.Id}]: {formatter(state, exception)}");
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}