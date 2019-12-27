using System;
using MonitorDesktop.Api;

namespace MonitorDesktop.Sockets.Tests
{
    internal class MockConnectionConsumer : ConnectionConsumerBase<IConfiguration>
    {
        private readonly Action<ConnectionObservation> _connectionCallback;
        private readonly Action<MessageObservation> _messageCallback;
        private readonly IConnection _connection;

        public MockConnectionConsumer(
            IConnection connection,
            IConfiguration configuration,
            Action<ConnectionObservation> connectionCallback,
            Action<MessageObservation> messageCallback) : base(connection, configuration)
        {
            _connection = connection;

            _connectionCallback = connectionCallback;
            _messageCallback = messageCallback;
            
            _connection.ConnectionChanged.Subscribe(_connectionCallback);
            _connection.MessageReceived.Subscribe(_messageCallback);
        }

        public override void Start() => _connection.Start();

        public override void Dispose() => _connection.Dispose();
    }
}