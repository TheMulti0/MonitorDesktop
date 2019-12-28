using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;
using MonitorDesktop.Client.Sockets;
using MonitorDesktop.Extensions;
using LoggerFactoryExtensions = MonitorDesktop.Extensions.LoggerFactoryExtensions;

namespace MonitorDesktop.Client
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            ILoggerFactory loggerFactory = LoggerFactoryExtensions.CreateDefault();
            
            var config = await JsonExtensions
                .ReadJsonAsync<ClientConfiguration>("appconfig.json");

            var factory = new ClientWebSocketFactory(
                loggerFactory.CreateLogger<ClientWebSocket>(),
                config.Host,
                config.Port);
            IConnection connection = factory.Create();

            var client = new Client(
                loggerFactory.CreateLogger<Client>(),
                connection,
                config);
            client.Start();

            await Task.Delay(-1);
        }
    }
}
