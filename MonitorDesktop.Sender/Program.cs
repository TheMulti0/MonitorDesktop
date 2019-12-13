using System.Diagnostics;
using System.Threading.Tasks;

namespace MonitorDesktop.Client
{
    public class Program
    {
        private static Task Main(string[] args) => new Program().Main();

        public Task Main()
        {
            new MonitorDesktopClient().Start();

            return Task.Delay(-1);
        }
    }
}