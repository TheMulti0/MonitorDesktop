using System;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Operational base-class that serves as a protocol-specific connection,
    /// has push-based events,
    /// supports synchronous sending of bytes of data 
    /// </summary>
    /// <see cref="IOperational" />
    public abstract class ConnectionBase : IOperational
    {
        public abstract IObservable<ConnectionObservation> ConnectionChanged { get; }
        public abstract IObservable<MessageObservation> MessageReceived { get; }

        protected ConnectionBase(IConfiguration configuration)
        {
        }

        public abstract void Start();
        
        public abstract void Send(byte[] message);
        
        public abstract void Dispose();

    }
}