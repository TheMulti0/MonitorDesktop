using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MonitorDesktop.Shared;
using WebSocketSharp;

namespace MonitorDesktop.Server
{
    internal class Program
    {
        private static void Main(string[] args) => new Program().Main();

        public void Main()
        {
            var server = new ReactiveWebSocketServer("ws://localhost:3000");

            server.Start();

            server.AddEndpoint(
                "/",
                listener => 
                    listener.OnOpenEvent.Subscribe(unit => Initialize(listener)));

            Console.ReadKey();
        }

        private void Initialize(IReactiveSocketListener listener)
        {
            listener.OnMessageEvent.Subscribe(OnNext);
        }

        private void OnNext(MessageEventArgs e)
        {
            var a = new MemoryStream(e.RawData);
            var img = Image.FromStream(a);
            img.Save($"poop-{DateTime.Now.Ticks}.png", ImageFormat.Png);
        }
    }
}