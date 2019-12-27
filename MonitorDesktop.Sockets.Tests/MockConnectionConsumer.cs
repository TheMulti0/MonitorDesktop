using System;
using MonitorDesktop.Api;

namespace MonitorDesktop.Sockets.Tests
{
    internal class MockConnectionConsumer : ConnectionConsumerBase<IConfiguration>
    {
        private readonly IConnection _connection;

        public MockConnectionConsumer(
            IConnection connection,
            IConfiguration configuration,
            Action<ConnectionObservation> connectionCallback,
            Action<MessageObservation> messageCallback) : base(connection, configuration)
        {
            _connection = connection;

            _connection.ConnectionChanged.Subscribe(connectionCallback);
            _connection.MessageReceived.Subscribe(messageCallback);
        }

        public override void Start() => _connection.Start();

        public override void Dispose() => _connection.Dispose();
    }
}