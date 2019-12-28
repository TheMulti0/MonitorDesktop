using System;

namespace MonitorDesktop.Reactive
{
    public static class Result
    {
        public static IResult<TSuccess, TFailure> FromSuccess<TSuccess, TFailure>(TSuccess success) 
            => new Result<TSuccess, TFailure>(success);
        
        public static IResult<TSuccess, TFailure> FromFailure<TSuccess, TFailure>(TFailure failure)
            => new Result<TSuccess, TFailure>(failure);
    }
    
    internal class Result<TSuccess, TFailure> : IResult<TSuccess, TFailure>
    {
        private readonly IOptional<TSuccess> _success;
        private readonly IOptional<TFailure> _failure;

        internal Result(TSuccess success)
        {
            _success = Optional.FromValue(success);
            
            _failure = Optional.Empty<TFailure>();
        }

        internal Result(TFailure failure)
        {
            _success = Optional.Empty<TSuccess>();
            
            _failure = Optional.FromValue(failure);
        }
        
        public void Do(
            Action<TSuccess> successConsumer,
            Action<TFailure> failureConsumer)
        {
            _success.Do(
                successConsumer,
                () => _failure.Do(
                    failureConsumer,
                    () => { }));
        }

        public T Map<T>(
            Func<TSuccess, T> successMapper,
            Func<TFailure, T> failureMapper)
        {
            return _success.Map(
                successMapper,
                () => _failure.Map(
                    failureMapper,
                    () => default(T)));
        }
    }
}