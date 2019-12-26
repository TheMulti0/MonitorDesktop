using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using MonitorDesktop.Api;

namespace MonitorDesktop.Client
{
    public class Client : ConnectionConsumerBase<ClientConfiguration>
    {
        private readonly ConnectionBase _connection;
        private readonly ClientConfiguration _configuration;
        private IDisposable? _timer;

        public Client(
            ConnectionBase connection,
            ClientConfiguration configuration) : base(connection, configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public override void Start()
        {
            _connection.Start();

            _connection
                .ConnectionChanged
                .Subscribe(
                    OnConnection,
                    e => OnDisconnection(),
                    OnDisconnection);
        }

        public override void Dispose() => _connection.Dispose();

        private void OnConnection(ConnectionObservation args) => _timer = Observable
            .Interval(
                TimeSpan.FromSeconds(1 / _configuration.FramesPerSecond))
            .Subscribe(interval => SendMessage());

        private void SendMessage()
        {
            var stream = new MemoryStream();
            GdiCapture
                .CaptureScreen()
                .Save(stream, ImageFormat.Png);

            _connection.Send(stream.ToArray());
        }

        private void OnDisconnection() => _timer?.Dispose();
    }
}