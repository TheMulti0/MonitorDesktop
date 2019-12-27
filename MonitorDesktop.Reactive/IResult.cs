using System;

namespace MonitorDesktop.Reactive
{
    public interface IResult<out TSuccess, out TFailure>
    {
        bool HasValue { get; }

        void Do(Action<TSuccess> successConsumer, Action<TFailure> failureConsumer);
    }
}