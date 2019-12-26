using System;
using Optionally;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Arguments for a push-based connection changed event 
    /// </summary>
    public class ConnectionObservation
    {
        public IResult<Exception, ConnectionState> Info { get; }

        public ConnectionObservation(IResult<Exception, ConnectionState> info)
        {
            Info = info;
        }
    }
}