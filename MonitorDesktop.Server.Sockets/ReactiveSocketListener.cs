using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class ReactiveSocketListener : WebSocketBehavior
    {
        private readonly Subject<ConnectionObservation> _connection = new Subject<ConnectionObservation>();
        private readonly Subject<MessageObservation> _message = new Subject<MessageObservation>();

        public IObservable<ConnectionObservation> Connection => _connection;
        public IObservable<MessageObservation> Message => _message;

        protected override void OnOpen() => _connection.OnNext(new ConnectionObservation(Context.RequestUri));
        protected override void OnClose(CloseEventArgs e) => _connection.OnCompleted();
        protected override void OnMessage(MessageEventArgs e) => _message.OnNext(new MessageObservation(e.RawData));
        protected override void OnError(ErrorEventArgs e) => _connection.OnError(e.Exception);
    }
}