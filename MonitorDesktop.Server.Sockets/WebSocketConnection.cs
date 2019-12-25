using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class WebSocketConnection : IConnection
    {
        private readonly Subject<ConnectionObservation> _connection = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _message = new Subject<MessageObservation>();
        private WebSocketServer _server;

        public IObservable<ConnectionObservation> Connection => _connection;
        public IObservable<MessageObservation> Message => _message;

        public void Initialize(IConfiguration configuration)
        {
            var url = configuration.MakeUri("ws")
                .ToString();
            _server = new WebSocketServer(url);
            _server.AddWebSocketService<ReactiveSocketListener>(
                "/",
                listener =>
                {
                    listener.Connection.Subscribe(
                        _connection.OnNext,
                        _connection.OnError,
                        _connection.OnCompleted);

                    listener.Message.Subscribe(
                        _message.OnNext,
                        _message.OnError,
                        _message.OnCompleted);
                });
        }

        public void Start() => _server.Start();

        public void Send(byte[] message)
            => throw new InvalidOperationException("WebSocketSharp server does not support sending messages");

        public void Dispose()
        {
            _connection.Dispose();
            _message.Dispose();
            
            _server.Stop();
        }
    }
}