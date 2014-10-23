using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace RGendarme.Settings
{
    [SettingsKey(typeof(CodeInspectionSettings), "RGendarme settings")]
    public class RGendarmeSettings
    {
        [SettingsEntry(true, "Whether to do or not")]
        public bool SampleOption { get; set; }
    }
}