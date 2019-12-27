using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using MonitorDesktop.Api;
using MonitorDesktop.Reactive;

namespace MonitorDesktop.Tests.Shared
{
    public class MockConnection : IConnection
    {
        private readonly Subject<ConnectionObservation> _connectionChanged = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _messageReceived = new Subject<MessageObservation>();
        private readonly IDisposable _timer;

        public IObservable<ConnectionObservation> ConnectionChanged => _connectionChanged;
        public IObservable<MessageObservation> MessageReceived => _messageReceived;

        public MockConnection(TimeSpan messageInterval, int messageCount, CancellationToken token)
        {
            _timer = Observable
                .Interval(messageInterval)
                .Take(messageCount)
                .Subscribe(
                    l => _messageReceived.OnNext(new MessageObservation(new byte[] { })));

            token.Register(_timer.Dispose);
        }

        public void Start() =>
            _connectionChanged.OnNext(
                new ConnectionObservation(Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        public void Send(byte[] message) => throw new NotImplementedException();

        public void Dispose() => _timer.Dispose();
    }
}