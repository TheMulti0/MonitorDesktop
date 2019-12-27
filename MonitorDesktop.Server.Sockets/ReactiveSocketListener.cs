using System;
using System.Reactive.Subjects;
using MonitorDesktop.Api;
using MonitorDesktop.Extensions;
using MonitorDesktop.Reactive;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MonitorDesktop.Server.Sockets
{
    public class ReactiveSocketListener : WebSocketBehavior
    {
        private readonly Subject<ConnectionObservation> _connection = new Subject<ConnectionObservation>();
        private readonly Subject<Message> _message = new Subject<Message>();

        public IObservable<ConnectionObservation> Connection => _connection;
        public IObservable<Message> Message => _message;

        protected override void OnOpen() =>
            _connection.OnNext(
                new ConnectionObservation(
                    Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        protected override void OnClose(CloseEventArgs e) =>
            _connection.OnNext(
                new ConnectionObservation(
                Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Disconnected)));

        protected override void OnMessage(MessageEventArgs e) 
            => _message.OnNext(e.RawData.Deserialize<Message>());

        protected override void OnError(ErrorEventArgs e) =>
            _connection.OnNext(
                new ConnectionObservation(Result.FromFailure<ConnectionState, Exception>(e.Exception)));
    }
}