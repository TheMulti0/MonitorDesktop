using System;
using MonitorDesktop.Api;

namespace MonitorDesktop.Sockets.Tests
{
    public class MockConnectionConsumer : IOperational
    {
        private readonly IConnection _connection;

        public MockConnectionConsumer(
            IConnection connection,
            Action<ConnectionObservation> connectionCallback,
            Action<MessageObservation> messageCallback)
        {
            _connection = connection;

            _connection.ConnectionChanged.Subscribe(connectionCallback);
            _connection.MessageReceived.Subscribe(messageCallback);
        }

        public void Start() => _connection.Start();

        public void Dispose() => _connection.Dispose();
    }
}