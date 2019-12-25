using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using Websocket.Client;

namespace MonitorDesktop.Client.Sockets
{
    public class WebSocketConnection : IConnection
    {
        private readonly Subject<ConnectionObservation> _connection;
        private WebsocketClient _client;

        public IObservable<ConnectionObservation> Connection => _connection;
        public IObservable<MessageObservation> Message { get; private set; }

        public WebSocketConnection() 
            => _connection = new Subject<ConnectionObservation>();

        public void Initialize(IConfiguration configuration)
        {
            Uri url = configuration.MakeUri("ws");
            _client = new WebsocketClient(url);

            Message = _client
                .MessageReceived
                .Select(message => new MessageObservation(message.Binary));

            _client
                .ReconnectionHappened
                .Subscribe(_ => _connection.OnNext(new ConnectionObservation(url)));
            
            _client
                .DisconnectionHappened
                .Subscribe(OnDisconnection);

        }
        
        public void Start() => _client.Start();

        public void Send(byte[] message) => _client.SendInstant(message);

        public void Dispose()
        {
            _connection?.Dispose();
            _client?.Dispose();
        }

        private void OnDisconnection(DisconnectionInfo info)
        {
            switch (info.Type)
            {
                case DisconnectionType.Error:
                    _connection.OnError(info.Exception);
                    break;

                default:
                    _connection.OnCompleted();
                    break;
            }
        }
    }
}