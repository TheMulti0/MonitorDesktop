using System;
using System.Reactive;
using WebSocketSharp;

namespace MonitorDesktop.Shared
{
    public interface IReactiveConnectionHost
    {
        IObservable<Unit> OnOpenEvent { get; }
        IObservable<CloseEventArgs> OnCloseEvent { get; }
        IObservable<MessageEventArgs> OnMessageEvent { get; }
        IObservable<ErrorEventArgs> OnErrorEvent { get; }
    }
}