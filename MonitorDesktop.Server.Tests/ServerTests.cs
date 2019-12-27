using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using MonitorDesktop.Tests.Shared;

namespace MonitorDesktop.Server.Tests
{
    [TestClass]
    public class ServerTests
    {
        private const int MessagesCount = 10;
        private const double Delay = 10;
        private readonly TimeSpan _messageInterval = TimeSpan.FromMilliseconds(10);
        private readonly string _imagesPath = "test-images";

        private Server _server;

        [TestMethod]
        public void TestImageArrival()
        {
            var config = new ServerConfiguration()
            {
                Host = "localhost",
                Port = 3000,
                ImagesPath = _imagesPath
            };
            
            var cts = new CancellationTokenSource();

            var factory = new MockConnectionFactory(_messageInterval, MessagesCount, cts.Token);
            IConnection connection = factory.Create();

            _server = new Server(connection, config);
            _server.Start();

            TimeSpan waitTime = _messageInterval * Delay * MessagesCount;
            
            Thread.Sleep(waitTime);
            
            OnNext();
            
        }

        private void OnNext()
        {
            _server.Dispose();
            int fileCount = Directory.GetFiles(_imagesPath).Length;
            Directory.Delete(_imagesPath, true);
            Assert.AreEqual(MessagesCount, fileCount);
        }
    }
}