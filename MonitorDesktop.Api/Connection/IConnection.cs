using System;

namespace MonitorDesktop.Api
{
    public interface IConnection : IDisposable
    {
        IObservable<ConnectionObservation> Connection { get; }
        IObservable<MessageObservation> Message { get; }

        void Initialize(IConfiguration configuration);

        void Start();
        
        void Send(byte[] message);
    }
}