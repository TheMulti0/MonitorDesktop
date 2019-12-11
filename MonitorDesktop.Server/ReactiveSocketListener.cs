using System;
using System.Reactive;
using System.Reactive.Subjects;
using WebSocketSharp;
using WebSocketSharp.Server;
using MonitorDesktop.Shared;

namespace MonitorDesktop.Server
{
    internal class ReactiveSocketListener : WebSocketBehavior, IReactiveSocketListener
    {
        private readonly Subject<CloseEventArgs> _onCloseSubject = new Subject<CloseEventArgs>();
        private readonly Subject<ErrorEventArgs> _onErrorSubject = new Subject<ErrorEventArgs>();
        private readonly Subject<MessageEventArgs> _onMessageSubject = new Subject<MessageEventArgs>();
        private readonly Subject<Unit> _onOpenSubject = new Subject<Unit>();

        public IObservable<Unit> OnOpenEvent => _onOpenSubject;
        public IObservable<CloseEventArgs> OnCloseEvent => _onCloseSubject;
        public IObservable<MessageEventArgs> OnMessageEvent => _onMessageSubject;
        public IObservable<ErrorEventArgs> OnErrorEvent => _onErrorSubject;

        protected override void OnOpen() => _onOpenSubject.OnNext(Unit.Default);
        protected override void OnClose(CloseEventArgs e) => _onCloseSubject.OnNext(e);
        protected override void OnMessage(MessageEventArgs e) => _onMessageSubject.OnNext(e);
        protected override void OnError(ErrorEventArgs e) => _onErrorSubject.OnNext(e);
    }
}