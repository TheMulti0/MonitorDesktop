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
            var config = await JsonExtensions
                .ReadJsonAsync<ClientConfiguration>("appconfig.json");

            var factory = new WebSocketConnectionFactory(config.Host, config.Port);
            IConnection connection = factory.Create();

            var client = new Client(connection, config);
            client.Start();

            await Task.Delay(-1);
        }
    }
}
