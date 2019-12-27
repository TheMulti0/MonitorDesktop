using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MonitorDesktop.Api;

namespace MonitorDesktop.Extensions
{
    public static class JsonExtensions
    {
        public static ValueTask<T> ReadJsonAsync<T>(string path)
            => JsonSerializer.DeserializeAsync<T>(new FileStream(path, FileMode.Open));
    }
}