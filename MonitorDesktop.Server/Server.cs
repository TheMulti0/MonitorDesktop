using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;

namespace MonitorDesktop.Server
{
    public class Server : IOperational
    {
        private readonly ILogger<Server> _logger;
        private readonly IConnection _connection;
        private readonly ServerConfiguration _configuration;

        public Server(
            ILogger<Server> logger,
            IConnection connection,
            ServerConfiguration configuration)
        {
            _logger = logger;
            _connection = connection;
            _configuration = configuration;
            
            _logger.LogInformation("Initialized");
        }

        public void Start()
        {
            if (!Directory.Exists(_configuration.ImagesPath))
            {
                Directory.CreateDirectory(_configuration.ImagesPath);
            }
            
            _connection.ConnectionChanged.Subscribe(OnConnection);

            _connection.Start();
            _logger.LogInformation("Started");
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposed");
            _connection.Dispose();
        }

        private void OnConnection(ConnectionInfo info)
        {
            info.State.Do(
                state =>
                {
                    switch (state)
                    {
                        case ConnectionState.Connected:
                            _logger.LogInformation($"'{state}' connection event occured");                            
                            break;
                    }
                },
                exception => _logger.LogError(exception, "Disconnected with an exception"));
            _connection.MessageReceived.SubscribeAsync(OnMessageAsync);
        }

        private Task OnMessageAsync(Message message)
        {
            _logger.LogInformation($"Received message from {message}");
            
            string path = Path.Combine(_configuration.ImagesPath, $"{DateTime.Now.Ticks}.png");
            
            _logger.LogInformation($"Saving data as image at {path}");
            
            return File.WriteAllBytesAsync(
                path,
                message.Data);
        }
    }
}