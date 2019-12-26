using System;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Interface holding operation control methods - Start and Stop (Dispose)
    /// </summary>
    public interface IOperational : IDisposable
    {
        void Start();
    }
}