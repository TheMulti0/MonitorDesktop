using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using LoggerFactoryExtensions = MonitorDesktop.Extensions.LoggerFactoryExtensions;

namespace MonitorDesktop.Client.Tests
{
    [TestClass]
    public class ClientTests
    {
        private const int TotalMessages = 10;
        private const int ImagesPerSecond = 30;
        private const int Delay = 10;
        private readonly object _imagesLock = new object();
        private readonly ILoggerFactory _loggerFactory = LoggerFactoryExtensions.CreateDefault();
        private int _imagesCount;

        [TestMethod]
        public void TestClientSnapshots()
        {
            Client client = GetClient();
            client.Start();

            Thread.Sleep(TimeSpan.FromSeconds(1.0 / ImagesPerSecond * TotalMessages * Delay));
            lock (_imagesLock)
            {
                Assert.AreEqual(TotalMessages, _imagesCount);
            }
        }

        private Client GetClient()
            => new Client(
                _loggerFactory.CreateLogger<Client>(),
                GetConnection(),
                GetConfig());

        private MockClient GetConnection()
        {
            MockClient connection = new MockClientFactory(TotalMessages).Create();
            connection.MessageSent.Subscribe(OnMessageSent);
            return connection;
        }

        private static ClientConfiguration GetConfig() =>
            new ClientConfiguration
            {
                Host = "localhost",
                Port = 3000,
                FramesPerSecond = ImagesPerSecond
            };

        private void OnMessageSent(Message message)
        {
            lock (_imagesLock)
            {
                _imagesCount++;
            }
        }
    }
}