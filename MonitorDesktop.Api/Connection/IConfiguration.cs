namespace MonitorDesktop.Api
{
    public interface IConfiguration
    {
        string Host { get; }

        int Port { get; }
    }
}