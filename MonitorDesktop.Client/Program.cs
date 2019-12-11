using System;
using System.Drawing.Imaging;
using System.IO;

namespace MonitorDesktop.Client
{
    internal class Program
    {
        private const double FramesPerSecond = 30;
        private ReactiveWebSocketClient _client;

        private static void Main(string[] args) 
            => new Program().Main();

        public void Main()
        {
            _client = new ReactiveWebSocketClient("ws://localhost:3000/poop");
            _client
                .OnOpenEvent
                .Subscribe(unit => Initialize());

            SendImage(2);

            Console.ReadKey();
        }

        private void Initialize()
        {
//            Observable
//                .Timer(
//                    TimeSpan.Zero,
//                    TimeSpan.FromMilliseconds(1000 / FramesPerSecond))
//                .Subscribe(SendImage);
        }

        private void SendImage(long l)
        {
            var image = GDICapture.CaptureScreen();

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            _client.SendAsync(
                stream,
                (int) stream.Length);
        }
    }
}