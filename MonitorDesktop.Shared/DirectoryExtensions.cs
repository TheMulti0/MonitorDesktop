using System.IO;

namespace MonitorDesktop.Shared
{
    public static class DirectoryExtensions
    {
        public static string GetProjectPath() 
            => Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\");
    }
}
