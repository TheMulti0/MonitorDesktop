using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class WebSocketConnection : IConnection
    {
        private readonly Subject<ConnectionInfo> _connectionChanged = new Subject<ConnectionInfo>();
        private readonly Subject<Message> _messageReceived = new Subject<Message>();
        private readonly WebSocketServer _server;

        public IObservable<ConnectionInfo> ConnectionChanged => _connectionChanged;
        public IObservable<Message> MessageReceived => _messageReceived;

        public WebSocketConnection(string host, int port)
        {
            _server = new WebSocketServer($"ws://{host}:{port}");
            _server.AddWebSocketService<ReactiveSocketListener>(
                "/",
                listener =>
                {
                    listener.Connection.Subscribe(
                        _connectionChanged.OnNext,
                        _connectionChanged.OnError,
                        _connectionChanged.OnCompleted);

                    listener.Message.Subscribe(
                        _messageReceived.OnNext,
                        _messageReceived.OnError,
                        _messageReceived.OnCompleted);
                });
        }

        public void Start() => _server.Start();

        public void Send(Message message)
            => throw new InvalidOperationException("WebSocketSharp server does not support sending messages");

        public void Dispose()
        {
            _connectionChanged.Dispose();
            _messageReceived.Dispose();
            
            _server.Stop();
        }
    }
}