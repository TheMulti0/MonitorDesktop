using System;

namespace MonitorDesktop.Api
{
    public class ConnectionObservation
    {
        public Uri Uri { get; set; }

        public ConnectionObservation(Uri uri)
        {
            Uri = uri;
        }
    }
}