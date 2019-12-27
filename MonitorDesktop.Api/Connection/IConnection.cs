using System;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Operational interface that serves as a protocol-specific connection,
    /// has push-based events,
    /// supports synchronous sending of bytes of data 
    /// </summary>
    /// <see cref="IOperational" />
    public interface IConnection : IOperational
    {
        IObservable<ConnectionObservation> ConnectionChanged { get; }
        IObservable<Message> MessageReceived { get; }
        
        void Send(Message message);
    }
}