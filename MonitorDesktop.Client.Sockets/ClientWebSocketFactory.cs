using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;

namespace MonitorDesktop.Client.Sockets
{
    public class ClientWebSocketFactory : IConnectionFactory<ClientWebSocket>
    {
        private readonly ILogger<ClientWebSocket> _logger;
        private readonly string _host;
        private readonly int _port;

        public ClientWebSocketFactory(
            ILogger<ClientWebSocket> logger,
            string host,
            int port)
        {
            _logger = logger;
            _host = host;
            _port = port;
        }

        public ClientWebSocket Create() => new ClientWebSocket(_logger, _host, _port);
    }
}