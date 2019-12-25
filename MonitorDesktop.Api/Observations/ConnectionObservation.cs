using System;

namespace MonitorDesktop.Api
{
    public class ConnectionObservation
    {
        public Uri Uri { get; }

        public ConnectionObservation(Uri uri)
        {
            Uri = uri;
        }
    }
}