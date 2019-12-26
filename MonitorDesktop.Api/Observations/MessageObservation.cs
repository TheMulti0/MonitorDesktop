namespace MonitorDesktop.Api
{
    /// <summary>
    /// Arguments for a push-based message received event
    /// </summary>
    public class MessageObservation
    {
        public byte[] Data { get; }
     
        public MessageObservation(byte[] data)
        {
            Data = data;
        }
    }
}