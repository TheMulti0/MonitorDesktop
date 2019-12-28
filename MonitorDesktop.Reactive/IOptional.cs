using System;

namespace MonitorDesktop.Reactive
{
    public interface IOptional<out TValue>
    {
        void Do(Action<TValue> valueConsumer, Action nullConsumer);
        
        T Map<T>(Func<TValue, T> valueMapper, Func<T> defaultMapper);
    }
}