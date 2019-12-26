using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorDesktop.Api;
using MonitorDesktop.Server.Tests;

namespace MonitorDesktop.Server.Sockets.Tests
{
    [TestClass]
    public class SocketTests
    {
        [TestMethod]
        public void TestUno()
        {
            IConfiguration configuration = new TestsConfiguration("localhost", 3000);
            ConnectionBase connection = new WebSocketConnection(configuration);
            
            
        }
    }
}