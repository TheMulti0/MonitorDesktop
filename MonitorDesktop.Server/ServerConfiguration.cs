using MonitorDesktop.Api;

namespace MonitorDesktop.Server
{
    public class ServerConfiguration : IConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string ImagesPath { get; set; }
    }
}