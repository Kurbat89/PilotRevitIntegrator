using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PilotRevitAddin.SynchronizeProject
{
    public static class SettingsPath
    {
        //public static string SynchronizeSettingsPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //    @"Resources\SynchronizeSettings.json");

        public static string SynchronizeSettingsPath { get; } = @"C:\Users\kurbatov.PCS\AppData\Local\TestProject\SynchronizeSettings.json";
    }
}
