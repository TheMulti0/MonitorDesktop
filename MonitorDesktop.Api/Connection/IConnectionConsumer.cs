namespace MonitorDesktop.Api
{
    public interface IConnectionConsumer<in TConfiguration> where TConfiguration : IConfiguration
    {
        void Initialize(
            IConnection connection,
            TConfiguration configuration);

        void Start();
    }
}