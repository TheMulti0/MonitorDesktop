using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using MonitorDesktop.Shared;
using WebSocketSharp;

namespace MonitorDesktop.Server
{
    class Program
    {
        private ReactiveWebSocketServer _server;

        private static void Main(string[] args) 
            => new Program().Main();

        public void Main()
        {
            using (_server = new ReactiveWebSocketServer("ws://localhost:3000"))
            {
                Console.ReadKey();
                _server
                    .OnOpenEvent
                    .Subscribe(unit => Initialize());

                _server.OnMessageEvent.Subscribe(OnNext);

                Console.ReadKey();
            }
        }

        private void Initialize()
        {
            _server.OnMessageEvent.Subscribe(OnNext);
        }

        private void OnNext(MessageEventArgs e)
        {
            var a = new MemoryStream(e.RawData);
            var img = Image.FromStream(a);
            img.Save("poop.png", ImageFormat.Png);
        }
    }
}
