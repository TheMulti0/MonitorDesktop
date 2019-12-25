using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MonitorDesktop.Api;

namespace MonitorDesktop.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Uri MakeUri(this IConfiguration config, string protocol)
            => new Uri($"{protocol}://{config.Host}:{config.Port}");

        public static ValueTask<T> ReadConfigurationAsync<T>(string path)
            where T : IConfiguration
            => JsonSerializer.DeserializeAsync<T>(new FileStream(path, FileMode.Open));
    }
}