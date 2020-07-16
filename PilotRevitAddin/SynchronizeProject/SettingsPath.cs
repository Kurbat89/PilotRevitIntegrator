using System.IO;
using System.Reflection;

namespace PilotRevitAddin.SynchronizeProject
{
    public static class SettingsPath
    {
        public static string SynchronizeSettingsPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"Resources\SynchronizeSettings.json");
    }
}
