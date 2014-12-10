using JetBrains.ReSharper.PowerToys.OptionsPage;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace RGendarme.Settings.Design
{
    /// <summary>
    /// Interaction logic for DesignOptionPage.xaml
    /// </summary>
    [OptionsPage(PID, "Design rules", typeof(OptionsPageThemedIcons.UseIntMaxValueOptions), ParentId = SampleOptionPage.PID)]
    public partial class DesignOptionPage : IOptionsPage
    {
        private const string PID = "DesignOptionPage";

        public DesignOptionPage()
        {
            InitializeComponent();

            // TODO: add bingings later
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
