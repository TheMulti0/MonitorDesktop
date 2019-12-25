using System;
using System.Text.Json.Serialization;
using MonitorDesktop.Api;

namespace MonitorDesktop.Client
{
    public class ClientConfiguration : IConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public double FramesPerSecond { get; set; }
    }
}