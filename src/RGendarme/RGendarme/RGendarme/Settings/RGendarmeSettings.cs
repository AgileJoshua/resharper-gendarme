using JetBrains.Application.Settings;

namespace RGendarme.Settings
{
    [SettingsKey(typeof(EnvironmentSettings), "RGendarme settings")]
    public class RGendarmeSettings
    {
        [SettingsEntry(true, "Whether to do or not")]
        public bool SampleOption { get; set; }
    }
}