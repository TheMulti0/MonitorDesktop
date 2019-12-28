using System;

namespace MonitorDesktop.Reactive
{
    public interface IOptional<out TValue>
    {
        bool HasValue { get; }
        
        void Do(Action<TValue> valueConsumer, Action nullConsumer);
        
        T Map<T>(Func<TValue, T> valueMapper, Func<T> defaultMapper);
    }
}