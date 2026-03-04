using System.Globalization;
using System.Net;
using System.Reflection;

namespace ILSCREEN_UI.Models
{
    public static class ServerInfoModel
    {
        public static string? ControllerName { get; set; }
        public static string AspNetCoreEnvironment { get; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString() ?? "";
        public static string NameServer => GetIPAddress();
        public static string AssemblyVersion => GetVersion();
        public static string AssemblyDateModified => GetDateModified();

        private static string GetIPAddress()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).HostName;
            }
            catch
            {
                return "";
            }
        }

        private static string GetDateModified()
        {
            var assemblyExe = Assembly.GetExecutingAssembly();
            FileInfo file = new FileInfo(assemblyExe.Location);
            return file.LastWriteTime.ToString("dd/MM/yyyy - HH:mm:ss", new CultureInfo("en-EN", false));
        }

        private static string GetVersion()
        {
            return Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "";
        }
    }
}
