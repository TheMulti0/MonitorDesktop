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
        public bool HasValue { get; }

        private IOptional<TSuccess> Success { get; }

        private IOptional<TFailure> Failure { get; }

        internal Result(TSuccess success)
        {
            Success = Optional.FromValue(success);
            HasValue = true;
            
            Failure = Optional.Empty<TFailure>();
        }

        internal Result(TFailure failure)
        {
            Success = Optional.Empty<TSuccess>();
            HasValue = false;
            
            Failure = Optional.FromValue(failure);
        }
        
        public void Do(
            Action<TSuccess> successConsumer,
            Action<TFailure> failureConsumer)
        {
            Success.Do(
                successConsumer,
                () => Failure.Do(
                    failureConsumer,
                    () => { }));
        }

        public T Map<T>(
            Func<TSuccess, T> successMapper,
            Func<TFailure, T> failureMapper)
        {
            return Success.Map(
                successMapper,
                () => Failure.Map(
                    failureMapper,
                    () => default(T)));
        }
    }
}