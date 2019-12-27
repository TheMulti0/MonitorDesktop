using System.Threading.Tasks;
using MonitorDesktop.Api;
using MonitorDesktop.Client.Sockets;
using MonitorDesktop.Extensions;

namespace MonitorDesktop.Client
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var config = await ConfigurationExtensions
                .ReadConfigurationAsync<ClientConfiguration>("appconfig.json");

            var factory = new WebSocketConnectionFactory(config);
            IConnection connection = factory.Create();

            var client = new Client(connection, config);
            client.Start();

            await Task.Delay(-1);
        }
    }
}
