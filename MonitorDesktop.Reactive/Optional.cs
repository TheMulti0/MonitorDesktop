using System;

namespace MonitorDesktop.Reactive
{
    public static class Optional
    {
        public static IOptional<T> FromValue<T>(T value) => new Optional<T>(value);

        public static IOptional<T> Empty<T>() => new Optional<T>();
    }
    
    internal class Optional<TValue> : IOptional<TValue>
    {
        private readonly TValue _value;

        public bool HasValue { get; }
        
        internal Optional(TValue value)
        {
            _value = value;
            HasValue = true;
        }

        internal Optional()
        {
            HasValue = false;
        }
        
        public void Do(Action<TValue> valueConsumer, Action nullConsumer)
        {
            if (HasValue)
            {
                valueConsumer(_value);
            }
            else
            {
                nullConsumer();
            }
        }
    }
}