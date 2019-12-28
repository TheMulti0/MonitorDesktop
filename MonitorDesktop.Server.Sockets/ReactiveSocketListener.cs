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
        private readonly Subject<ConnectionInfo> _connection = new Subject<ConnectionInfo>();
        private readonly Subject<Message> _message = new Subject<Message>();

        public IObservable<ConnectionInfo> Connection => _connection;
        public IObservable<Message> Message => _message;

        protected override void OnOpen() =>
            _connection.OnNext(
                new ConnectionInfo(
                    Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Connected)));

        protected override void OnClose(CloseEventArgs e) =>
            _connection.OnNext(
                new ConnectionInfo(
                Result.FromSuccess<ConnectionState, Exception>(ConnectionState.Disconnected)));

        protected override void OnMessage(MessageEventArgs e) 
            => _message.OnNext(e.RawData.Deserialize<Message>());

        protected override void OnError(ErrorEventArgs e) =>
            _connection.OnNext(
                new ConnectionInfo(Result.FromFailure<ConnectionState, Exception>(e.Exception)));
    }
}