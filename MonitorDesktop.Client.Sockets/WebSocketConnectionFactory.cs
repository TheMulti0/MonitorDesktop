using MonitorDesktop.Api;

namespace MonitorDesktop.Client.Sockets
{
    public class WebSocketConnectionFactory : IConnectionFactory<WebSocketConnection>
    {
        private readonly string _host;
        private readonly int _port;

        public WebSocketConnectionFactory(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public WebSocketConnection Create() => new WebSocketConnection(_host, _port);
    }
}