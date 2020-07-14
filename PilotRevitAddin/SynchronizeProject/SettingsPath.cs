using System.IO;

namespace PilotRevitAddin.SynchronizeProject
{
    public static class SettingsPath
    {
        //public static string SynchronizeSettingsPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //    @"Resources\SynchronizeSettings.json");

        public static string SynchronizeSettingsPath { get; } = Path.Combine(Directory.GetCurrentDirectory(),
            @"Resources\SynchronizeSettings.json");

        // public static string SynchronizeSettingsPath { get; } = @"C:\Users\kurbatov.PCS\AppData\Local\TestProject\SynchronizeSettings.json";
    }
}
