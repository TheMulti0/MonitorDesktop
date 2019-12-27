using System;
using System.IO;
using System.Threading.Tasks;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;

namespace MonitorDesktop.Server
{
    public class Server : ConnectionConsumerBase<ServerConfiguration>
    {
        private readonly IConnection _connection;
        private readonly ServerConfiguration _configuration;

        public Server(
            IConnection connection,
            ServerConfiguration configuration) : base(connection, configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public override void Start()
        {
            if (!Directory.Exists(_configuration.ImagesPath))
            {
                Directory.CreateDirectory(_configuration.ImagesPath);
            }
            
            _connection.Start();
            
            _connection.ConnectionChanged.Subscribe(OnConnection);
        }

        public override void Dispose() => _connection.Dispose();

        private void OnConnection(ConnectionObservation observation)
        {
            _connection.MessageReceived.SubscribeAsync(OnMessageAsync);
        }

        private Task OnMessageAsync(MessageObservation message)
        {
            DateTime now = DateTime.Now;
            string combine = Path.Combine(_configuration.ImagesPath, $"{now.Ticks}.png");
            return File.WriteAllBytesAsync(
                combine,
                message.Data);
        }
    }
}