using System;
using System.Reactive;
using MonitorDesktop.Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server
{
    public class ReactiveWebSocketServer : IReactiveSocketListener, IDisposable
    {
        private readonly WebSocketServer _server;

        public IObservable<Unit> OnOpenEvent { get; private set; }
        public IObservable<CloseEventArgs> OnCloseEvent { get; private set; }
        public IObservable<MessageEventArgs> OnMessageEvent { get; private set; }
        public IObservable<ErrorEventArgs> OnErrorEvent { get; private set; }

        public ReactiveWebSocketServer(string url)
        {
            _server = new WebSocketServer(url);
            
            _server.AddWebSocketService<ReactiveSocketListener>("/poop", reactive =>
            {
                OnOpenEvent = reactive.OnOpenEvent;
                OnCloseEvent = reactive.OnCloseEvent;
                OnMessageEvent = reactive.OnMessageEvent;
                OnErrorEvent = reactive.OnErrorEvent;
            });

            _server.Start();
        }

        public void Dispose() => _server.Stop();
    }
}