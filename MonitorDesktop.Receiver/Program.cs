using System.Threading.Tasks;

namespace MonitorDesktop.Receiver
{
    internal class Program
    {
        public static Task Main(string[] args) => new Program().Main();

        internal Task Main()
        {
            new MonitorDesktopReceiver().Start();
            return Task.Delay(-1);
        }
    }
}