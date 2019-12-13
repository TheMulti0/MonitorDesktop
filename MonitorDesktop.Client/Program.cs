using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Shared;

namespace MonitorDesktop.Client
{
    internal class Program
    {
        private const double FramesPerSecond = 30;

        private Settings _settings;
        private ILogger<Program> _logger;
        private ReactiveWebSocketClient _client;

        private static Task Main(string[] args) => new Program().Main();

        public Task Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var config = ReadConfiguration();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddConfiguration(config));

            _logger = loggerFactory.CreateLogger<Program>();

            _settings = config.GetSection("Settings").Get<Settings>();
            _client = new ReactiveWebSocketClient(
                $"ws://{_settings.Host}:{_settings.Port}",
                loggerFactory.CreateLogger<ReactiveWebSocketClient>());

            stopWatch.Stop();
            _logger.LogInformation($"Initialized in {stopWatch.Elapsed.TotalSeconds}s");

            _client.Connect();
            Initialize();

            return Task.Delay(-1);
        }

        private static IConfigurationRoot ReadConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(DirectoryExtensions.GetProjectPath())
                .AddJsonFile("appconfig.json")
                .Build();

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

            _logger.LogInformation($"Screenshot snapped at { DateTime.Now }");

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            _client.SendAsync(
                stream,
                (int) stream.Length);
        }
    }
}