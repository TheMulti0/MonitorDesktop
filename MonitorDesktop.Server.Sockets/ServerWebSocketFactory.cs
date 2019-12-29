using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;

namespace MonitorDesktop.Server.Sockets
{
    public class ServerWebSocketFactory : IConnectionFactory<ServerWebSocket>
    {
        private readonly ILogger<ServerWebSocket> _logger;
        private readonly string _host;
        private readonly int _port;

        public ServerWebSocketFactory(
            ILogger<ServerWebSocket> logger,
            string host,
            int port)
        {
            _logger = logger;
            _host = host;
            _port = port;
        }

        public ServerWebSocket Create() => new ServerWebSocket(_logger, _host, _port);
    }
}