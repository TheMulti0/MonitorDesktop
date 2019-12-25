﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Linq;
using MonitorDesktop.Api;

namespace MonitorDesktop.Client
{
    public class Client
    {
        private readonly IConnection _connection;
        private readonly ClientConfiguration _configuration;
        private IDisposable? _timer;

        public Client(
            IConnection connection,
            ClientConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public void Start()
        {
            _connection.Start();

            _connection
                .Connection
                .Subscribe(
                    OnConnection,
                    e => OnDisconnection(),
                    OnDisconnection);
        }

        private void OnConnection(ConnectionObservation args)
        {
            _timer = Observable
                .Interval(
                    TimeSpan.FromSeconds(1 / _configuration.FramesPerSecond))
                .Subscribe(interval => SendMessage());
        }

        private void SendMessage()
        {
            var stream = new MemoryStream();
            GdiCapture
                .CaptureScreen()
                .Save(stream, ImageFormat.Png);
            
            _connection.Send(stream.ToArray());
        }

        private void OnDisconnection()
        {
            _timer?.Dispose();
        }
    }
}