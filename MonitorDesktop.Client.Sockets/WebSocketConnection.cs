using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using Optionally;
using Websocket.Client;

namespace MonitorDesktop.Client.Sockets
{
    public class WebSocketConnection : ConnectionBase
    {
        private readonly Subject<ConnectionObservation> _connection;
        private readonly WebsocketClient _client;

        public override IObservable<ConnectionObservation> ConnectionChanged => _connection;
        public override IObservable<MessageObservation> MessageReceived { get; }

        public WebSocketConnection(IConfiguration configuration) : base(configuration)
        {
            _connection = new Subject<ConnectionObservation>();

            Uri uri = configuration.MakeUri("ws");
            _client = new WebsocketClient(uri);

            MessageReceived = _client
                .MessageReceived
                .Select(message => new MessageObservation(message.Binary));

            _client
                .ReconnectionHappened
                .Subscribe(
                    _ => _connection.OnNext(
                        new ConnectionObservation(
                            Result.Success<Exception, ConnectionState>(
                                ConnectionState.Connected))));

            _client
                .DisconnectionHappened
                .Subscribe(OnDisconnection);
        }

        public override void Start() => _client.Start();

        public override void Send(byte[] message) => _client.SendInstant(message);

        public override void Dispose()
        {
            _connection?.Dispose();
            _client?.Dispose();
        }

        private void OnDisconnection(DisconnectionInfo info)
        {
            switch (info.Type)
            {
                case DisconnectionType.Error:
                    _connection.OnNext(
                        new ConnectionObservation(
                            Result.Failure<Exception, ConnectionState>(info.Exception)));
                    break;

                default:
                    _connection.OnNext(
                        new ConnectionObservation(
                            Result.Success<Exception, ConnectionState>(
                                ConnectionState.Disconnected)));
                    break;
            }
        }
    }
}