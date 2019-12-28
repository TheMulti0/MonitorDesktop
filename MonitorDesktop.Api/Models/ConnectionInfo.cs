using System;
using MonitorDesktop.Reactive;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Arguments for a push-based connection changed event 
    /// </summary>
    public class ConnectionInfo
    {
        public IResult<ConnectionState, Exception> State { get; }

        public ConnectionInfo(IResult<ConnectionState, Exception> state)
        {
            State = state;
        }
    }
}