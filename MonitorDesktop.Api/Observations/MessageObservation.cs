namespace MonitorDesktop.Api
{
    public class MessageObservation
    {
        public byte[] Data { get; }
     
        public MessageObservation(byte[] data)
        {
            Data = data;
        }
    }
}