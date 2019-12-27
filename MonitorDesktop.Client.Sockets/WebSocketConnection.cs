using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using MonitorDesktop.Reactive;
using Websocket.Client;

namespace MonitorDesktop.Client.Sockets
{
    public class WebSocketConnection : IConnection
    {
        private readonly Subject<ConnectionObservation> _connection;
        private readonly WebsocketClient _client;

        public IObservable<ConnectionObservation> ConnectionChanged => _connection;
        public IObservable<Message> MessageReceived { get; }

        internal WebSocketConnection(string host, int port)
        {
            _connection = new Subject<ConnectionObservation>();

            var uri = new Uri($"ws://{host}:{port}");
            _client = new WebsocketClient(uri);

            MessageReceived = _client
                .MessageReceived
                .Select(message => message.Binary.Deserialize<Message>());

            _client
                .ReconnectionHappened
                .Subscribe(
                    _ => _connection.OnNext(
                        new ConnectionObservation(
                            Result.FromSuccess<ConnectionState, Exception>(
                                ConnectionState.Connected))));

            _client
                .DisconnectionHappened
                .Subscribe(OnDisconnection);
        }

        public void Start() => _client.Start();

        public void Send(Message message) => _client.SendInstant(message.Serialize());

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
                    _connection.OnNext(
                        new ConnectionObservation(
                            Result.FromFailure<ConnectionState, Exception>(info.Exception)));
                    break;

                default:
                    _connection.OnNext(
                        new ConnectionObservation(
                            Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Disconnected)));
                    break;
            }
        }
    }
}