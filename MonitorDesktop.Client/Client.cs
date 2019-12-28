using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using MonitorDesktop.Api;

namespace MonitorDesktop.Client
{
    public class Client : IOperational
    {
        private readonly ILogger<Client> _logger;
        private readonly IConnection _connection;
        private readonly ClientConfiguration _configuration;
        private IDisposable? _timer;

        public Client(
            ILogger<Client> logger,
            IConnection connection,
            ClientConfiguration configuration)
        {
            _logger = logger;
            _connection = connection;
            _configuration = configuration;

            _logger.LogInformation("Initialized");
        }

        public void Start()
        {
            _connection
                .ConnectionChanged
                .Subscribe(
                    OnConnectionChanged,
                    e => OnDisconnection(),
                    OnDisconnection);

            _connection.Start();
            _logger.LogInformation("Started");
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _connection.Dispose();
            _logger.LogInformation("Disposed");
        }

        private void OnConnectionChanged(ConnectionInfo args) =>
            args.State.Do(
                state =>
                {
                    switch (state)
                    {
                        case ConnectionState.Connected:
                            OnConnection(state);
                            break;
                        default:
                            OnDisconnection(state);
                            break;
                    }
                },
                OnDisconnection);

        private void OnConnection(ConnectionState state)
        {
            _logger.LogInformation($"'{state}' connection event occured");
            _timer = Observable
                .Interval(
                    TimeSpan.FromSeconds(1 / _configuration.FramesPerSecond))
                .Subscribe(interval => SendMessage());
        }

        private void SendMessage()
        {
            _logger.LogInformation("Sending screenshot");
            var stream = new MemoryStream();
            GdiCapture
                .CaptureScreen()
                .Save(stream, ImageFormat.Png);

            _connection.Send(
                new Message(
                    GetType()
                        .Name,
                    stream.ToArray()));
        }

        private void OnDisconnection(ConnectionState state)
        {
            _logger.LogError($"'{state}' disconnection event occured");
            OnDisconnection();
        }

        private void OnDisconnection(Exception exception)
        {
            _logger.LogError(exception, "Disconnected with an exception");
            OnDisconnection();
        }

        private void OnDisconnection() => _timer?.Dispose();
    }
}