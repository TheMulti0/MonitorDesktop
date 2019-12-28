using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using MonitorDesktop.Server.Sockets;

namespace MonitorDesktop.Sockets.Tests
{
    [TestClass]
    public class SocketsCommunicationTests
    {
        private const int TotalMessagesCount = 5;

        private readonly TimeSpan _messagesInterval = TimeSpan.FromMilliseconds(1);
        private readonly TestsConfiguration _testsConfig = new TestsConfiguration("localhost", 3000);
        private readonly object _messagesLock = new object();

        private int _messagesCount;

        [TestMethod]
        public void TestClientToServerCommunication()
        {
            StartServer();
            StartClient();

            WaitForMessages();
        }

        private void WaitForMessages()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                if (HaveMessagesArrived())
                {
                    break;
                }
                ValidateTime(stopwatch);
                Thread.Sleep(_messagesInterval);
            }
        }

        private void ValidateTime(Stopwatch stopwatch)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            TimeSpan eta = TotalMessagesCount * _messagesInterval;
            const int communicationDelay = 50;
            if (elapsed <= eta * communicationDelay)
            {
                return;
            }

            lock (_messagesLock)
            {
                Assert.Fail(
                    $"Test took {elapsed} instead of {eta} * {communicationDelay}. \n" +
                    $"Got {_messagesCount} messages");
            }
        }

        private bool HaveMessagesArrived()
        {
            lock (_messagesLock)
            {
                return _messagesCount >= TotalMessagesCount;
            }
        }

        private void StartClient()
        {
            IConnection clientConnection = GetClientConnection();
            clientConnection.Start();
            clientConnection.ConnectionChanged.Subscribe(
                observation => OnClientConnectionChanged(observation, clientConnection));
        }

        private void StartServer()
        {
            IConnection serverConnection = GetServerConnection();
            MockConnectionConsumer server = GetMockServer(serverConnection);
            server.Start();
        }


        private MockConnectionConsumer GetMockServer(IConnection serverConnection)
            => new MockConnectionConsumer(
                serverConnection,
                _ => { },
                OnMessageReceived);

        private IConnection GetServerConnection()
            => new WebSocketConnectionFactory(_testsConfig.Host, _testsConfig.Port).Create();

        private IConnection GetClientConnection()
            => new Client.Sockets.WebSocketConnectionFactory(_testsConfig.Host, _testsConfig.Port).Create();

        private void OnClientConnectionChanged(ConnectionInfo info, IConnection connection)
            => info.State.Do(state => OnClientConnection(state, connection), _ => { });

        private void OnClientConnection(ConnectionState state, IConnection sender)
        {
            if (state == ConnectionState.Connected)
            {
                BeginSending(sender);
            }
        }

        private void BeginSending(IConnection connection)
            => Observable
                .Interval(_messagesInterval)
                .Take(TotalMessagesCount)
                .Subscribe(
                    _ => connection.Send(
                        new Message(
                            GetType().Name,
                            new byte[] { })));

        private void OnMessageReceived(Message info)
        {
            lock (_messagesLock)
            {
                _messagesCount++;
            }
        }
    }
}