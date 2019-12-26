using System;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using MonitorDesktop.Client.Sockets;

namespace MonitorDesktop.Sockets.Tests
{
    [TestClass]
    public class SocketsTests
    {
        private const int TotalMessagesCount = 5;

        private readonly TimeSpan _messagesInterval = TimeSpan.FromMilliseconds(1);
        private readonly IConfiguration _testsConfig = new TestsConfiguration("localhost", 3000);
        private readonly object _messagesLock = new object();

        private int _messagesCount;

        [TestMethod]
        public void TestClientToServerCommunication()
        {
            ConnectionBase serverConnection = GetServerConnection();
            ConnectionBase clientConnection = GetClientConnection();

            using MockConnectionConsumer server = GetMockServer(serverConnection);
            server.Start();

            clientConnection.Start();
            clientConnection.ConnectionChanged.Subscribe(
                observation => OnClientConnectionChanged(observation, clientConnection));

            while (true)
            {
                lock (_messagesLock)
                {
                    if (_messagesCount >= TotalMessagesCount)
                    {
                        break;
                    }
                }
                Thread.Sleep(_messagesInterval);
            }
        }

        private void BeginSending(ConnectionBase connection)
            => Observable
                .Interval(_messagesInterval)
                .Take(TotalMessagesCount)
                .Subscribe(
                    _ => connection.Send(
                        new byte[]
                        {
                        }));


        private MockConnectionConsumer GetMockServer(ConnectionBase serverConnection)
            => new MockConnectionConsumer(
                serverConnection,
                _testsConfig,
                _ => { },
                OnMessageReceived);

        private ConnectionBase GetClientConnection()
            => new WebSocketConnection(_testsConfig);

        private ConnectionBase GetServerConnection()
            => new Server.Sockets.WebSocketConnection(_testsConfig);

        private void OnClientConnectionChanged(ConnectionObservation observation, ConnectionBase connection)
            => observation.Info.Do(_ => { }, state => OnClientConnection(state, connection));

        private void OnClientConnection(ConnectionState state, ConnectionBase sender)
        {
            if (state == ConnectionState.Connected)
            {
                BeginSending(sender);
            }
        }

        private void OnMessageReceived(MessageObservation info)
        {
            lock (_messagesLock)
            {
                _messagesCount++;
            }
        }
    }
}