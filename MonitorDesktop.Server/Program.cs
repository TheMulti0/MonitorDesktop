using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using MonitorDesktop.Shared;
using WebSocketSharp;

namespace MonitorDesktop.Server
{
    internal class Program
    {
        private readonly string _imagesPath;

        public static Task Main(string[] args) => new Program().Main();

        public Program()
        {
            _imagesPath = Path.Combine(DirectoryExtensions.GetProjectPath(), "images");
        }


        internal Task Main()
        {
            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }

            var server = new ReactiveWebSocketServer("ws://localhost:3000");
            server.Start();
            server.AddEndpoint(
                "/",
                listener =>
                {
                    listener.OnCloseEvent.Subscribe(OnClose);
                    listener.OnMessageEvent.Subscribe(OnMessageReceived);
                });

            return Task.Delay(-1);
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