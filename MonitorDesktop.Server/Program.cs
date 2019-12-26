using System.Threading.Tasks;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using MonitorDesktop.Server.Sockets;

namespace MonitorDesktop.Server
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var config = await ConfigurationExtensions
                .ReadConfigurationAsync<ServerConfiguration>("appconfig.json");

            ConnectionBase connection = new WebSocketConnection(config);

            var server = new Server(connection, config);
            server.Start();

            await Task.Delay(-1);
        }
    }
}
