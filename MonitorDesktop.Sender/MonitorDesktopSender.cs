using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Sender;
using MonitorDesktop.Shared;

namespace MonitorDesktop.Client
{
    public class MonitorDesktopSender
    {
        private readonly ReactiveWebSocketClient _client;
        private readonly Settings _settings;
        private readonly ILogger<Program> _logger;

        public MonitorDesktopSender()
        {
            var config = ReadConfiguration();
            
            var loggerFactory = LoggerFactory.Create(
                builder => builder.AddConsole().AddConfiguration(config));

            _logger = loggerFactory.CreateLogger<Program>();

            _settings = config.GetSection("Settings").Get<Settings>();
            _client = new ReactiveWebSocketClient(
                $"ws://{_settings.Host}:{_settings.Port}",
                loggerFactory.CreateLogger<ReactiveWebSocketClient>());
        }

        private static IConfigurationRoot ReadConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(DirectoryExtensions.GetProjectPath())
                .AddJsonFile("appconfig.json")
                .Build();

        public void Start()
        {
            _logger.LogInformation("Initialized");

            _client.Connect();

            Observable
                .Interval(
                    TimeSpan.FromSeconds(1000 / _settings.FramesPerSecond))
                .Subscribe(_ => SendScreenshot());
        }

        private void SendScreenshot()
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