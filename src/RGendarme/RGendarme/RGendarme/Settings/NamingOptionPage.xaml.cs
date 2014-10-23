using JetBrains.DataFlow;
using JetBrains.ReSharper.PowerToys.OptionsPage;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace RGendarme.Settings
{
    /// <summary>
    /// Interaction logic for NamingOptionPage.xaml
    /// </summary>
    [OptionsPage(PID, "Naming rules", typeof(OptionsPageThemedIcons.UseIntMaxValueOptions), ParentId = SampleOptionPage.PID)]
    public partial class NamingOptionPage : IOptionsPage
    {
        private const string PID = "NamingOptionPage";

        public NamingOptionPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            InitializeComponent();
            // TODO add bindings
        }

        public bool OnOk()
        {
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control { get { return this; } }
        public string Id { get { return PID; } }
    }
}
