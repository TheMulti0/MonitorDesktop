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
        private readonly bool _hasValue;

        
        internal Optional(TValue value)
        {
            _value = value;
            _hasValue = true;
        }

        internal Optional()
        {
            _value = default;
            _hasValue = false;
        }
        
        public void Do(
            Action<TValue> valueConsumer,
            Action nullConsumer)
        {
            if (_hasValue)
            {
                valueConsumer(_value);
            }
            else
            {
                nullConsumer();
            }
        }

        public T Map<T>(Func<TValue, T> valueMapper, Func<T> defaultMapper) 
            => _hasValue ? valueMapper(_value) : defaultMapper();
    }
}