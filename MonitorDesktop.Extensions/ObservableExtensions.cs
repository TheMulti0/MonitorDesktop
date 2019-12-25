using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace MonitorDesktop.Extensions
{
    public static class ObservableSubscriptions
    {
        public static IDisposable SubscribeAsync<T>(
            this IObservable<T> observable,
            Func<T, Task> onNext) =>
                observable
                    .Select(
                        e =>
                            Observable.Defer(
                                () => onNext(e)
                                    .ToObservable()))
                    .Concat()
                    .Subscribe(e => { });

        public static IDisposable SubscribeAsync<T>(
            this IObservable<T> observable,
            Func<T, Task> onNext,
            Action onCompleted) =>
                observable
                    .Select(
                        e =>
                            Observable.Defer(
                                () => onNext(e)
                                    .ToObservable()))
                    .Concat()
                    .Subscribe(
                        e => { },
                        onCompleted);

        public static IDisposable SubscribeAsync<T>(
            this IObservable<T> observable,
            Func<T, Task> onNext,
            Action<Exception> onError) =>
                observable
                    .Select(
                        e =>
                            Observable.Defer(
                                () => onNext(e)
                                    .ToObservable()))
                    .Concat()
                    .Subscribe(
                        e => { },
                        onError);

        public static IDisposable SubscribeAsync<T>(
            this IObservable<T> observable,
            Func<T, Task> onNext,
            Action<Exception> onError,
            Action onCompleted) =>
                observable
                    .Select(
                        e =>
                            Observable.Defer(
                                () => onNext(e)
                                    .ToObservable()))
                    .Concat()
                    .Subscribe(
                        e => { },
                        onError,
                        onCompleted);
    }
}