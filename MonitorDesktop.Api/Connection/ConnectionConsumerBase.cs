namespace MonitorDesktop.Api
{
    /// <summary>
    /// Operational base-class that holds a connection and custom configuration 
    /// </summary>
    /// <typeparam name="TConfiguration">Custom configuration implementation supplied by the consumer</typeparam>
    /// <see cref="IOperational" />
    public abstract class ConnectionConsumerBase<TConfiguration> : IOperational where TConfiguration : IConfiguration
    {
        protected ConnectionConsumerBase(
            ConnectionBase connection,
            TConfiguration configuration)
        {
        }

        public abstract void Start();

        public abstract void Dispose();
    }
}