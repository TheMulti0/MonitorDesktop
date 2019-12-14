using System.Threading.Tasks;
using MonitorDesktop.Shared;

namespace MonitorDesktop.Receiver
{
    internal class Program
    {
        public static Task Main(string[] args) => new Program().Main();

        internal Task Main()
        {
            new MonitorDesktopReceiver(DirectoryExtensions.GetProjectPath()).Start();
            return Task.Delay(-1);
        }
    }
}