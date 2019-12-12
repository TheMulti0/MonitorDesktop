using System;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server
{
    public class ReactiveWebSocketServer
    {
        private readonly WebSocketServer _server;

        public ReactiveWebSocketServer(string url) => _server = new WebSocketServer(url);

        public void Start() => _server.Start();

        public void AddEndpoint(string path, Action<ReactiveSocketListener> listenerSubscriber) 
            => _server.AddWebSocketService<ReactiveSocketListener>(path, listenerSubscriber);

        public void Stop() => _server.Stop();
    }
}