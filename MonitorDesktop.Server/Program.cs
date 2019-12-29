using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using MonitorDesktop.Server.Sockets;
using LoggerFactoryExtensions = MonitorDesktop.Extensions.LoggerFactoryExtensions;

namespace MonitorDesktop.Server
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            ILoggerFactory loggerFactory = LoggerFactoryExtensions.CreateDefault();
            
            var config = await JsonExtensions
                .ReadJsonAsync<ServerConfiguration>("appconfig.json");

            var factory = new ServerWebSocketFactory(
                loggerFactory.CreateLogger<ServerWebSocket>(),
                config.Host,
                config.Port);
            IConnection connection = factory.Create();

            var server = new Server(
                loggerFactory.CreateLogger<Server>(),
                connection,
                config);
            server.Start();

            await Task.Delay(-1);
        }
    }
}
