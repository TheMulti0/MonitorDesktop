using System;
using System.IO;
using MonitorDesktop.Shared;
using WebSocketSharp;

namespace MonitorDesktop.Receiver
{
    public class MonitorDesktopReceiver
    {
        private readonly string _imagesPath;
        private readonly ReactiveWebSocketServer _server;

        public MonitorDesktopReceiver()
        {
            _imagesPath = Path.Combine(DirectoryExtensions.GetProjectPath(), "images");
            _server = new ReactiveWebSocketServer("ws://localhost:3000");
        }

        public void Start()
        {
            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }

            _server.Start();

            _server.AddEndpoint("/", RegisterEvents);
        }

        private void RegisterEvents(ReactiveSocketListener listener)
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