using System;
using System.IO;
using System.Threading.Tasks;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;

namespace MonitorDesktop.Server
{
    public class Server 
    {
        private readonly IConnection _connection;
        private readonly ServerConfiguration _configuration;

        public Server(
            IConnection connection,
            ServerConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public void Start()
        {
            if (!Directory.Exists(_configuration.ImagesPath))
            {
                Directory.CreateDirectory(_configuration.ImagesPath);
            }
            
            _connection.Start();
            
            _connection.Connection.Subscribe(OnConnection);
        }

        private void OnConnection(ConnectionObservation observation)
        {
            _connection.Message.SubscribeAsync(OnMessageAsync);
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