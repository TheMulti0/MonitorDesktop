using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Shared;
using WebSocketSharp;

namespace MonitorDesktop.Receiver
{
    public class MonitorDesktopReceiver
    {
        private readonly string _imagesPath;
        private readonly ReactiveWebSocketServer _server;
        private readonly ILogger<MonitorDesktopReceiver> _logger;
        private readonly Settings _settings;
        private readonly string _url;

        public MonitorDesktopReceiver(string configPath)
        {
            var config = ReadConfiguration(configPath);

            _logger = LoggerFactory
                .Create(builder => builder
                    .AddConsole()
                    .AddConfiguration(config))
                .CreateLogger<MonitorDesktopReceiver>();

            _settings = config.GetSection("Settings").Get<Settings>();
            _imagesPath = Path.Combine(DirectoryExtensions.GetProjectPath(), _settings.ImagesPath);

            _url = $"ws://{_settings.Host}:{_settings.Port}";
            _server = new ReactiveWebSocketServer(_url);
        }

        private static IConfigurationRoot ReadConfiguration(string configPath) =>
            new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appconfig.json")
                .Build();

        public void Start()
        {
            _logger.LogInformation("Initialized receiver");

            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
                _logger.LogInformation($"Created new empty directory at /{_settings.ImagesPath}");
            }

            _server.Start();
            _logger.LogInformation("Server started");

            _server.AddEndpoint("/", RegisterEvents);
            _logger.LogInformation($"Registered endpoint at / ({_url})");
        }

        private void RegisterEvents(ReactiveSocketHost listener)
        {
            listener.OnCloseEvent.Subscribe(OnClose);
            listener.OnMessageEvent.Subscribe(OnMessageReceived);
        }

        private void OnClose(CloseEventArgs args)
        {
            Directory.Delete(_imagesPath, true);
        }

        private void OnMessageReceived(MessageEventArgs args)
        {
            if (!args.IsBinary) return;

            var filename = Path.Combine(_imagesPath, $"poop-{DateTime.Now.Ticks}.png");
            File.WriteAllBytesAsync(filename, args.RawData);
        }
    }
}