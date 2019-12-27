using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Reactive;

namespace MonitorDesktop.Client.Tests
{
    public class MockClient : IConnection
    {
        private readonly Subject<ConnectionObservation> _connectionChanged = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _messageReceived = new Subject<MessageObservation>();
        private readonly Subject<byte[]> _messageSent = new Subject<byte[]>();
        private readonly int _messagesCount;
        private int _currentMessagesCount;
        public IObservable<byte[]> MessageSent => _messageSent;

        public MockClient(int messagesCount) => _messagesCount = messagesCount;

        public IObservable<ConnectionObservation> ConnectionChanged => _connectionChanged;
        public IObservable<MessageObservation> MessageReceived => _messageReceived;

        public void Start()
            =>
                _connectionChanged.OnNext(
                    new ConnectionObservation(Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        public void Send(byte[] message)
        {
            if (_currentMessagesCount < _messagesCount)
            {
                _messageSent.OnNext(message);
            }
            _currentMessagesCount++;
        }

        public void Dispose()
        {
        }
    }
}