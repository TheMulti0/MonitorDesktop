namespace MonitorDesktop.Api
{
    /// <summary>
    /// Interface of factories of IConnection implementations
    /// </summary>
    /// <typeparam name="TConnection">Implementation specific connection</typeparam>
    /// <see cref="IConnection" />
    public interface IConnectionFactory<out TConnection> where TConnection : IConnection
    {
        TConnection Create();
    }
}