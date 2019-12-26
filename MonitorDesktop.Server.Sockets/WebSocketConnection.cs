using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class WebSocketConnection : ConnectionBase
    {
        private readonly Subject<ConnectionObservation> _connectionChanged = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _messageReceived = new Subject<MessageObservation>();
        private readonly WebSocketServer _server;

        public override IObservable<ConnectionObservation> ConnectionChanged => _connectionChanged;
        public override IObservable<MessageObservation> MessageReceived => _messageReceived;

        public WebSocketConnection(IConfiguration configuration) : base(configuration)
        {
            var url = configuration.MakeUri("ws")
                .ToString();
            _server = new WebSocketServer(url);
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

        public override void Start() => _server.Start();

        public override void Send(byte[] message)
            => throw new InvalidOperationException("WebSocketSharp server does not support sending messages");

        public override void Dispose()
        {
            _connectionChanged.Dispose();
            _messageReceived.Dispose();
            
            _server.Stop();
        }
    }
}