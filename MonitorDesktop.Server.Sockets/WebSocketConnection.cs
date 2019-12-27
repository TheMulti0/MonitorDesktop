using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class WebSocketConnection : IConnection
    {
        private readonly Subject<ConnectionObservation> _connectionChanged = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _messageReceived = new Subject<MessageObservation>();
        private readonly WebSocketServer _server;

        public IObservable<ConnectionObservation> ConnectionChanged => _connectionChanged;
        public IObservable<MessageObservation> MessageReceived => _messageReceived;

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

        public void Send(byte[] message)
            => throw new InvalidOperationException("WebSocketSharp server does not support sending messages");

        public void Dispose()
        {
            _connectionChanged.Dispose();
            _messageReceived.Dispose();
            
            _server.Stop();
        }
    }
}