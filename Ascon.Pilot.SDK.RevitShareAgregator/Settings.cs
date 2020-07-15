using System.ComponentModel.Composition;
using System.Windows;
using Ascon.Pilot.SDK;

namespace Ascon.Pilot.RevitShareAgregator
{
    public static class SettingsFeatureKeys
    {
        public static string RevitAggregatorProjectPathKey => "RevitShareSettings-59C7743C-4AD7-4004-81BD-93DAE4C0D31D";
        public static string RevitProjectInfoKey => "RevitShareSettings-9C7677F5-134B-4BA1-82AB-23CEDA9994CF";
    }

    [Export(typeof(ISettingsFeature))]
    public class RevitShareSettingsFeature : ISettingsFeature
    {
        public void SetValueProvider(ISettingValueProvider settingValueProvider) { }
        public string Key => SettingsFeatureKeys.RevitAggregatorProjectPathKey;
        public string Title => "Revit project path for Aggregator";
        public FrameworkElement Editor => null;
    }

    [Export(typeof(ISettingsFeature))]
    public class RevitProjectInfoFeature : ISettingsFeature
    {
        public void SetValueProvider(ISettingValueProvider settingValueProvider) { }
        public string Key => SettingsFeatureKeys.RevitProjectInfoKey;
        public string Title => "Revit project info attributes";
        public FrameworkElement Editor => null;
    }
}