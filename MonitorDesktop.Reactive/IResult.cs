using System;

namespace MonitorDesktop.Reactive
{
    public interface IResult<out TSuccess, out TFailure>
    {
        void Do(Action<TSuccess> successConsumer, Action<TFailure> failureConsumer);

        T Map<T>(Func<TSuccess, T> successMapper, Func<TFailure, T> failureMapper);
    }
}