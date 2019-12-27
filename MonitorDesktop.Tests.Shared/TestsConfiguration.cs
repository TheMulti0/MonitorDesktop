using MonitorDesktop.Api;

namespace MonitorDesktop.Tests.Shared
{
    public class TestsConfiguration
    {
        public string Host { get; }

        public int Port { get; }

        public TestsConfiguration(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}