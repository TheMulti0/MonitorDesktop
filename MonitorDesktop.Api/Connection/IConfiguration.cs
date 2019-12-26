namespace MonitorDesktop.Api
{
    /// <summary>
    /// High level configuration used by all connections
    /// </summary>
    public interface IConfiguration
    {
        string Host { get; }

        int Port { get; }
    }
}