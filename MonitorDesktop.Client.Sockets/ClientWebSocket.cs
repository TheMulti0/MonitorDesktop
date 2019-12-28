using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using MonitorDesktop.Reactive;
using Websocket.Client;
using Websocket.Client.Models;

namespace MonitorDesktop.Client.Sockets
{
    public class ClientWebSocket : IConnection
    {
        private readonly ILogger<ClientWebSocket> _logger;
        private readonly Subject<ConnectionInfo> _connection;
        private readonly WebsocketClient _client;

        internal ClientWebSocket(
            ILogger<ClientWebSocket> logger,
            string host,
            int port)
        {
            _logger = logger;
            _connection = new Subject<ConnectionInfo>();

            var uri = new Uri($"ws://{host}:{port}");
            _client = new WebsocketClient(uri);

            SubscribeToEvents();

            _logger.LogInformation("Initialized");
        }

        public IObservable<ConnectionInfo> ConnectionChanged => _connection;
        public IObservable<Message> MessageReceived { get; private set; }

        public void Start()
        {
            _client.Start();
            _logger.LogInformation("Started");
        }

        public void Send(Message message) => _client.SendInstant(message.Serialize());

        public void Dispose()
        {
            _connection?.Dispose();
            _client?.Dispose();
            _logger.LogInformation("Disposed");
        }

        private void SubscribeToEvents()
        {
            MessageReceived = _client.MessageReceived.Select(OnMessageReceived);
            _client.ReconnectionHappened.Subscribe(OnReconnection);
            _client.DisconnectionHappened.Subscribe(OnDisconnection);
        }

        private Message OnMessageReceived(ResponseMessage response)
        {
            var message = response.Binary.Deserialize<Message>();
            _logger.LogInformation($"Received a message from {message.Author}, created at {message.CreationDate}");
            return message;
        }

        private void OnReconnection(ReconnectionInfo info)
        {
            _logger.LogInformation($"'{info.Type}' reconnection event occured");
            _connection.OnNext(
                new ConnectionInfo(Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));
        }

        private void OnDisconnection(DisconnectionInfo info)
        {
            switch (info.Type)
            {
                case DisconnectionType.Error:
                    _logger.LogCritical(info.Exception, "Disconnected with an exception");
                    _connection.OnNext(
                        new ConnectionInfo(
                            Result.FromFailure<ConnectionState, Exception>(info.Exception)));
                    break;

                default:
                    _logger.LogCritical($"'{info.Type}' disconnection event occured");
                    _connection.OnNext(
                        new ConnectionInfo(
                            Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Disconnected)));
                    break;
            }
        }
    }
}