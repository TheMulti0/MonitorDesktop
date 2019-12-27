using System;

namespace MonitorDesktop.Api
{
    /// <summary>
    /// Arguments for a push-based message received event
    /// </summary>
    [Serializable]
    public class Message
    {
        public string Author { get; set; }

        public DateTime CreationDate { get; set; }
        
        public byte[] Data { get; }

        public Message(string author, byte[] data)
        {
            Author = author;
            CreationDate = DateTime.Now;
            Data = data;
        }
    }
}