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

            var factory = new WebSocketConnectionFactory(config);
            IConnection connection = factory.Create();

            var server = new Server(connection, config);
            server.Start();

            await Task.Delay(-1);
        }
    }
}
