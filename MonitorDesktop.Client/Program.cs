using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace MonitorDesktop.Client
{
    internal class Program
    {
        private const double FramesPerSecond = 30;
        
        private ReactiveWebSocketClient _client;
        private ILogger<Program> _logger;

        private static void Main(string[] args) => new Program().Main();

        public void Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<Program>();


            _client = new ReactiveWebSocketClient(
                "ws://localhost:3000/",
                loggerFactory.CreateLogger<ReactiveWebSocketClient>());
            
            stopWatch.Stop();
            _logger.LogInformation($"Initialized in {stopWatch.Elapsed.TotalSeconds}s");

            _client.Connect();
            Initialize();

            Console.ReadKey();
        }

        private void Initialize()
        {
            Observable
                .Interval(
                    TimeSpan.FromMilliseconds(1000 / FramesPerSecond))
                .Subscribe(l => SendImage());
        }

        private void SendImage()
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