using System;
using MonitorDesktop.Reactive;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Arguments for a push-based connection changed event 
    /// </summary>
    public class ConnectionObservation
    {
        public IResult<ConnectionState, Exception> Info { get; }

        public ConnectionObservation(IResult<ConnectionState, Exception> info)
        {
            Info = info;
        }
    }
}