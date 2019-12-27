using System;
using System.Reactive.Subjects;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MonitorDesktop.Api;
using MonitorDesktop.Reactive;
using Message = MonitorDesktop.Api.Message;

namespace MonitorDesktop.Client.Tests
{
    public class MockClient : IConnection
    {
        private readonly Subject<ConnectionObservation> _connectionChanged = new Subject<ConnectionObservation>();
        private readonly Subject<Message> _messageReceived = new Subject<Message>();
        private readonly Subject<Message> _messageSent = new Subject<Message>();
        private readonly int _messagesCount;
        private int _currentMessagesCount;

        public IObservable<ConnectionObservation> ConnectionChanged => _connectionChanged;
        public IObservable<Message> MessageReceived => _messageReceived;
        public IObservable<Message> MessageSent => _messageSent;

        public MockClient(int messagesCount) => _messagesCount = messagesCount;

        public void Start()
            =>
                _connectionChanged.OnNext(
                    new ConnectionObservation(Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        public void Send(Message message)
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