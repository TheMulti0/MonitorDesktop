using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Reactive;

namespace MonitorDesktop.Server.Tests
{
    public class MockReceivingServer : IConnection
    {
        private readonly Subject<ConnectionInfo> _connectionChanged = new Subject<ConnectionInfo>();
        private readonly Subject<Message> _messageReceived = new Subject<Message>();
        private readonly IDisposable _timer;

        public IObservable<ConnectionInfo> ConnectionChanged => _connectionChanged;
        public IObservable<Message> MessageReceived => _messageReceived;

        public MockReceivingServer(TimeSpan messageInterval, int messageCount)
        {
            _timer = Observable
                .Interval(messageInterval)
                .Take(messageCount)
                .Subscribe(
                    l => _messageReceived.OnNext(new Message(GetType().Name, new byte[] { })));
        }

        public void Start() =>
            _connectionChanged.OnNext(
                new ConnectionInfo(Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        public void Send(Message message) => throw new InvalidOperationException("This connection is to be used for receiving messages only.");

        public void Dispose() => _timer.Dispose();
    }
}