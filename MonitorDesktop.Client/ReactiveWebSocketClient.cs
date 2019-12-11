using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using MonitorDesktop.Shared;
using WebSocketSharp;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace MonitorDesktop.Client
{
    public class ReactiveWebSocketClient : IReactiveSocketListener, IDisposable
    {
        private readonly WebSocket _client;

        public IObservable<Unit> OnOpenEvent { get; private set; }
        public IObservable<CloseEventArgs> OnCloseEvent { get; private set; }
        public IObservable<MessageEventArgs> OnMessageEvent { get; private set; }
        public IObservable<ErrorEventArgs> OnErrorEvent { get; private set; }

        public ReactiveWebSocketClient(string url)
        {
            _client = new WebSocket(url);
            _client.Connect();
            
            CreateObservables();
        }

        private void CreateObservables()
        {
            OnOpenEvent = Observable
                .FromEventPattern(
                    v => _client.OnOpen += v,
                    v => _client.OnOpen -= v)
                .Select(pattern => Unit.Default);

            OnCloseEvent = Observable
                .FromEventPattern<CloseEventArgs>(
                    v => _client.OnClose += v,
                    v => _client.OnClose -= v)
                .Select(pattern => pattern.EventArgs);

            OnMessageEvent = Observable
                .FromEventPattern<MessageEventArgs>(
                    v => _client.OnMessage += v,
                    v => _client.OnMessage -= v)
                .Select(pattern => pattern.EventArgs);

            OnErrorEvent = Observable
                .FromEventPattern<ErrorEventArgs>(
                    v => _client.OnError += v,
                    v => _client.OnError -= v)
                .Select(pattern => pattern.EventArgs);
        }

        public void Dispose() => _client.CloseAsync();

        public void SendAsync(Stream stream, int length) 
            => _client.Send(stream, length);
    }
}