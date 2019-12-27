using System;
using System.Threading;
using MonitorDesktop.Api;

namespace MonitorDesktop.Tests.Shared
{
    public class MockConnectionFactory : IConnectionFactory<MockConnection>
    {
        private readonly TimeSpan _messageInterval;
        private readonly int _messageCount;
        private readonly CancellationToken _token;

        public MockConnectionFactory(TimeSpan messageInterval, int messageCount, CancellationToken token)
        {
            _messageInterval = messageInterval;
            _messageCount = messageCount;
            _token = token;
        }

        public MockConnection Create() => new MockConnection(_messageInterval, _messageCount, _token);
    }
}