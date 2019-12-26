using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using Optionally;

namespace MonitorDesktop.Server.Tests
{
    internal class MockClientConnection : ConnectionBase
    {
        private readonly Subject<ConnectionObservation> _connection = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _message = new Subject<MessageObservation>();

        private readonly TimeSpan _messageInterval;
        private readonly Func<byte[]> _dataSupplier;

        public override IObservable<ConnectionObservation> ConnectionChanged => _connection;
        public override IObservable<MessageObservation> MessageReceived => _message;

        public MockClientConnection(
            IConfiguration configuration,
            TimeSpan messageInterval,
            Func<byte[]> dataSupplier) : base(configuration)
        {
            _messageInterval = messageInterval;
            _dataSupplier = dataSupplier;
        }

        public override void Start()
        {
            _connection.OnNext(
                new ConnectionObservation(
                    Result.Success<Exception, ConnectionState>(
                        ConnectionState.Connected)));
            Observable
                .Interval(_messageInterval)
                .Subscribe(
                    l => _message
                        .OnNext(new MessageObservation(_dataSupplier())));
        }

        public override void Send(byte[] message)
        {
        }

        public override void Dispose()
        {
            _connection.Dispose();
            _message.Dispose();
        }
    }
}