using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonitorDesktop.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] Serialize<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }
            
            var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, obj);
            return stream.ToArray();
        }

        public static T Deserialize<T>(this byte[] data)
        {
            var stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return (T) new BinaryFormatter().Deserialize(stream);
        } 
    }
}