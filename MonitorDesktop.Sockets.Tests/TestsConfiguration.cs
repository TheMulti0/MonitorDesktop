namespace MonitorDesktop.Sockets.Tests
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