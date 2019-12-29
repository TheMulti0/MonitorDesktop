using System;
using System.Text.Json.Serialization;

namespace MonitorDesktop.Client
{
    public class ClientConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public double FramesPerSecond { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ReconnectionTimeout { get; set; }
    }
}