using MonitorDesktop.Api;

namespace MonitorDesktop.Client.Sockets
{
    public class WebSocketConnectionFactory : IConnectionFactory<WebSocketConnection>
    {
        private readonly IConfiguration _configuration;

        public WebSocketConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WebSocketConnection Create() => new WebSocketConnection(_configuration);
    }
}