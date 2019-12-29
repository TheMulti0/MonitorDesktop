using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using LoggerFactoryExtensions = MonitorDesktop.Extensions.LoggerFactoryExtensions;

namespace MonitorDesktop.Server.Tests
{
    [TestClass]
    public class ServerTests
    {
        private const string ImagesPath = "test-images";
        private const int MessagesCount = 10;
        private const double Delay = 10;
        private readonly TimeSpan _messageInterval = TimeSpan.FromMilliseconds(10);
        private readonly ILoggerFactory _loggerFactory = LoggerFactoryExtensions.CreateDefault();

        private Server _server;

        [TestMethod]
        public void TestImageArrival()
        {
            _server = GetServer();
            _server.Start();

            Thread.Sleep(_messageInterval * Delay * MessagesCount);

            int fileCount = Directory.GetFiles(ImagesPath)
                .Length;
            DisposeResources();
            Assert.AreEqual(MessagesCount, fileCount);
        }

        private void DisposeResources()
        {
            _server.Dispose();
            Directory.Delete(ImagesPath, true);
        }

        private Server GetServer()
            => new Server(
                _loggerFactory.CreateLogger<Server>(),
                GetMockConnection(),
                GetConfiguration());

        private IConnection GetMockConnection()
            => new MockReceivingServerFactory(_messageInterval, MessagesCount).Create();

        private static ServerConfiguration GetConfiguration() =>
            new ServerConfiguration
            {
                Host = "localhost",
                Port = 3000,
                ImagesPath = ImagesPath
            };
    }
}